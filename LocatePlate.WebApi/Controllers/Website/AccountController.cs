using LocatePlate.Infrastructure.Constant;
using LocatePlate.Infrastructure.Domain;
using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.Cms.Modules.Deals;
using LocatePlate.Model.Identity;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using LocatePlate.Service.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Website
{

    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly LocatePlateContext _locatePlateContext;
        private readonly IPageService _pageService;
        private readonly IOptions<Host> _hostOptions;


        public AccountController(SignInManager<UserIdentity> signInManager,
            UserManager<UserIdentity> userManager,
            IHttpContextAccessor contextAccessor,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IPageService pageService,
            IOptions<Host> hostOptions,
        LocatePlateContext locatePlateContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _locatePlateContext = locatePlateContext;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _pageService = pageService;
            _hostOptions = hostOptions;
        }

        [HttpGet("login/{returnurl?}")]
        public IActionResult Index(string returnurl)
        {
            return View(new User { ReturnUrl = string.IsNullOrEmpty(returnurl) ? Request.GetTypedHeaders()?.Referer?.AbsoluteUri : returnurl });
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            //EmailHelper.Email("info@izonnet.com", "Test Email", "This is test Email");
            ViewBag.Message = "";
            return View(new User { ReturnUrl = Request.GetTypedHeaders()?.Referer?.AbsoluteUri });
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model)
        {
            ModelState.Remove("UserName"); // This will remove the key
            ModelState.Remove("ConfirmPassword"); // This will remove the key 
            ModelState.Remove("PhoneNumber"); // This will remove the key 
            ModelState.Remove("ConfirmPassword"); // This will remove the key 
            ModelState.Remove("IsAcceptedTermAndCondition"); // This will remove the key 

            // if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                    return Redirect(model.ReturnUrl ?? "/");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If we got this far, something failed, redisplay form
            return View("Index", model);
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
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
                    ModelState.AddModelError(string.Empty, "Phone number already associated with one of locateplate account");
                    return View();
                }
                var IsExistingEmail = _locatePlateContext.UserIdentities.Any(c => c.Role == UserRoles.WebsiteUser &&
                 c.Email == user.Email);
                if (IsExistingEmail)
                {
                    ModelState.AddModelError(string.Empty, "Email already associated with one of locateplate account");
                    return View();
                }
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(UserRoles.WebsiteUser))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.WebsiteUser));

                    var idenityResult = await _userManager.AddToRoleAsync(user, UserRoles.WebsiteUser);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = $"{this._hostOptions.Value.BaseUrl}/account/confirmEmail?userId={user.Id}&code={code}&returnUrl={model.ReturnUrl}&type={0}";
                    //Url.Page(
                    //    "/account/confirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code, returnUrl = model.ReturnUrl },
                    //    protocol: Request.Scheme);

                    var email = EmailHelper.PrepareWelcomeEmail(model.UserName, callbackUrl, "support@locateplate.com");
                    email = email.Replace("#Deals#", getDeals());
                    email = email.Replace("#host#", this._hostOptions.Value.BaseUrl);
                    EmailHelper.Email(model.Email, "Confirm your email", email);

                    var userEntity = this._locatePlateContext.UserIdentities.FirstOrDefault(c => c.Id == user.Id);
                    userEntity.EmailConfirmed = true;
                    this._locatePlateContext.UserIdentities.Update(userEntity);
                    this._locatePlateContext.SaveChanges();

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        ViewBag.Message = "Email is sent, Please use the link in email to verify your account";
                        return View();
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        this._contextAccessor.HttpContext.Session.SetString(SessionContants.WebSiteUserId, user.Id);
                        return Redirect(model.ReturnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            // If we got this far, something failed, redisplay form
            return View();
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> confirmEmail(string userId, string code, string returnUrl,int type)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewBag.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            ViewBag.userType = type;
            return View("Index");
        }

        [HttpGet("IsUserNameExist")]
        public IActionResult IsUserNameExist(string userName)
        {
            var result = this._locatePlateContext.Users.Any(c => c.UserName.ToLower().Contains(userName.ToLower()));
            return Ok(result);
        }

        [HttpGet("IsUserNameExist")]
        public IActionResult IsEmailExist(string email)
        {
            var result = this._locatePlateContext.Users.Any(c => c.Email.ToLower().Contains(email.ToLower()));
            return Ok(result);
        }

        [HttpGet("IsUserNameExist")]
        public IActionResult IsPhoneNumberExist(string phone)
        {
            var result = this._locatePlateContext.Users.Any(c => c.PhoneNumber.ToLower().Contains(phone.ToLower()));
            return Ok(result);
        }


        private string getDeals()
        {
            string cityName;
            this._contextAccessor.HttpContext.Request.Cookies.TryGetValue("LocationName", out cityName);
            if (string.IsNullOrEmpty(cityName))
                cityName = LocationConstant.DefaultLocation;
            string dealsMarkup = @"<table class='module' role='module' data-type='spacer' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='2931446b-8b48-42bd-a70c-bffcfe784680.1'>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style='padding:0px 0px 20px 0px;' role='module-content' bgcolor='#4d5171'>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                            <table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' role='module' data-type='columns' style='padding:0px 20px 30px 20px;' bgcolor='#4d5171'>
                                                                                <tbody>
                                                                                    <tr role='module-content'>
                                                                                        <td height='100%' valign='top'>
                                                                                            <table class='column' width='265' style='width:265px; border-spacing:0; border-collapse:collapse; margin:0px 15px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td style='padding:0px;margin:0px;border-spacing:0;'>
                                                                                                            <table class='module' role='module' data-type='spacer' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='a45551e7-98d7-40da-889d-a0dc41550c4e'>
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style='padding:0px 0px 15px 0px;' role='module-content' bgcolor=''>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='8ZPkEyRmw35sXLUWrdumXA.1'>
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style='font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='center'>
                                                                                                                            <img class='max-width' border='0' style='display:block; color:#000000; text-decoration:none; font-family:Helvetica, arial, sans-serif; font-size:16px; max-width:100% !important; width:100%; height:auto !important;' width='265' alt='' data-proportionally-constrained='true' data-responsive='true' src='#Image#' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                            <table class='column' width='265' style='width:265px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 15px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td style='padding:0px;margin:0px;border-spacing:0;'>
                                                                                                            <table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='4vL54iw2MCdgWcxxaCgLhi.1'>
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style='padding:18px 0px 18px 0px; line-height:20px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'>
                                                                                                                            <div>
                                                                                                                                <div style='font-family: inherit; text-align: inherit'><span style='color: #9cfed4; font-size: 15px'><strong>#Name#</strong></span></div>
                                                                                                                                <div style='font-family: inherit; text-align: inherit'><br /></div>
                                                                                                                                <div style='font-family: inherit; text-align: inherit'><span style='color: #ffffff; font-size: 15px'>#Description#</span></div>
                                                                                                                                <div style='font-family: inherit; text-align: inherit'><br /></div>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>";
            var page = this._pageService.GetByUrl($"{cityName.ToLower()}");
            DealModule deals = null;
            try
            {
                deals = page?.PageLayout?.Section.FirstOrDefault(c => c?.Module?.DealModule != null).Module?.DealModule;
            }
            catch (System.Exception ex)
            { }
            var markup = "";
            if (deals != null)
            {
                foreach (var item in deals.Deals)
                {
                    markup += dealsMarkup.Replace("#Image#", $"#host#/CMS/{cityName.ToLower()}/{item.Images}").Replace("#Name#", item.Name).Replace("#Description#", item.Description);
                }

            }

            return markup;

        }
    }
}

