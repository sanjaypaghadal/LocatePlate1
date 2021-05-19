using LocatePlate.Infrastructure.Constant;
using LocatePlate.Infrastructure.Domain;
using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.Api;
using LocatePlate.Model.Identity;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using LocatePlate.WebApi.Contracts.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.BaseRoute.V1)]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly LocatePlateContext _locatePlateContext;
        private readonly IOptions<Host> _hostOptions;

        public AccountController(SignInManager<UserIdentity> signInManager,
            UserManager<UserIdentity> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor,
            IOptions<Host> hostOptions,
            LocatePlateContext locatePlateContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _locatePlateContext = locatePlateContext;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
            _hostOptions = hostOptions;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            ModelState.Remove("UserName"); // This will remove the key
            ModelState.Remove("ConfirmPassword"); // This will remove the key 
            ModelState.Remove("PhoneNumber"); // This will remove the key 
            ModelState.Remove("ConfirmPassword"); // This will remove the key 
            ModelState.Remove("IsAcceptedTermAndCondition"); // This will remove the key

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                var user = await _userManager.FindByNameAsync(model.Email);

                UpdateDeviceId(user, model.DeviceId);
                if (result.Succeeded) return Ok(new { user = user, message = "", IsStatus = true });
                else return Ok(new AccountResponseModel { User = null, Message = "Invalid login attempt." });
            }

            // If we got this far, something failed, redisplay form
            return Ok(new AccountResponseModel { User = null, Message = "Invalid login attempt." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User model)
        {
            ModelState.Remove("ConfirmPassword"); // This will remove the key 
            ModelState.Remove("IsAcceptedTermAndCondition"); // This will remove the key 

            if (ModelState.IsValid)
            {
                var user = new UserIdentity { BusinessName = model.UserName, UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, Role = UserRoles.WebsiteUser };

                var IsExistingPhone = _locatePlateContext.UserIdentities.Any(c => c.Role == UserRoles.WebsiteUser &&
                 c.PhoneNumber == user.PhoneNumber);
                if (IsExistingPhone)
                {
                    return Ok(new AccountResponseModel { User = null, Message = "Phone number already associated with one of locateplate account" });
                }
                var IsExistingEmail = _locatePlateContext.UserIdentities.Any(c => c.Role == UserRoles.WebsiteUser &&
                 c.Email == user.Email);
                if (IsExistingEmail)
                {
                    return Ok(new AccountResponseModel { User = null, Message = "Email already associated with one of locateplate account" });
                }
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(UserRoles.WebsiteUser))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.WebsiteUser));

                    var idenityResult = await _userManager.AddToRoleAsync(user, UserRoles.WebsiteUser);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = $"{this._hostOptions.Value.BaseUrl}/account/confirmEmail?userId={user.Id}&code={code}&returnUrl={model.ReturnUrl}";
                    //Url.Page(
                    //    "/account/confirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code, returnUrl = model.ReturnUrl },
                    //    protocol: Request.Scheme);

                    UpdateDeviceId(null, model.DeviceId, model.Email);


                    var email = EmailHelper.PrepareWelcomeEmail(model.UserName, callbackUrl, "support@locateplate.com");
                    // email = email.Replace("#Deals#", getDeals());
                    email = email.Replace("#host#", this._hostOptions.Value.BaseUrl);
                    EmailHelper.Email(model.Email, "Confirm your email", email);

                    var userEntity = this._locatePlateContext.UserIdentities.FirstOrDefault(c => c.Id == user.Id);
                    userEntity.EmailConfirmed = true;
                    this._locatePlateContext.UserIdentities.Update(userEntity);
                    this._locatePlateContext.SaveChanges();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return Ok(new AccountResponseModel { User = null, Message = "Email is sent, Please use the link in email to verify your account", IsStatus = true });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Ok(new AccountResponseModel { User = null, Message = "Email is sent, Please use the link in email to verify your account", IsStatus = true });
            }

            var errorString = "";
            var errorResultObj = from ms in ModelState
                                 where ms.Value.Errors.Any()
                                 let fieldKey = ms.Key
                                 let errors = ms.Value.Errors
                                 from error in errors
                                 select $"{fieldKey}, {error.ErrorMessage}";

            errorResultObj.ToList().ForEach(c => errorString += c);
            // If we got this far, something failed, redisplay form
            return Ok(new AccountResponseModel { User = null, Message = errorString });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public string RefreshToken(UserIdentity request)
        {
            var expirationTime = this._configuration.GetValue<int>("Authentication:ExpirationTime");
            var requestAt = DateTime.Now;
            var expiresIn = requestAt.Add(TimeSpan.FromMinutes(expirationTime));

            // var token = GenerateToken(new UserIdentity() { UserName = request.UserName, Password = request.Password }, expiresIn);
            return "asd";
        }

        private void UpdateDeviceId(UserIdentity user, string deviceId, string email = "")
        {

            if (user != null && !string.IsNullOrEmpty(deviceId))
            {
                if (!string.IsNullOrEmpty(email))
                    user = _locatePlateContext.UserIdentities.FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
                user.DeviceId = deviceId;
                user.IsAndroid = true;
                _locatePlateContext.UserIdentities.Update(user);
                _locatePlateContext.SaveChanges();
            }
        }

        //private string GenerateToken(UserIdentity user, DateTime expires)
        //{
        //    var handler = new JwtSecurityTokenHandler();

        //    ClaimsIdentity identity = new ClaimsIdentity(
        //        new GenericIdentity(user.UserName, "TokenAuth"),
        //        new[] {
        //            new Claim(ClaimValueTypes.String, user.UserName)
        //        }
        //    );

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Authentication:SecurityKey")));

        //    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        //    {
        //        Issuer = "Issuer",
        //        Audience = "Audience",
        //        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        //        Subject = identity,
        //        Expires = expires,
        //        NotBefore = DateTime.Now.Subtract(TimeSpan.FromMinutes(30))
        //    });
        //    return handler.WriteToken(securityToken);
        //}
    }
}