using LocatePlate.Infrastructure.Constant;
using LocatePlate.Infrastructure.Extentions;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using LocatePlate.Service.Pages;
using LocatePlate.Service.Restaurants;
using LocatePlate.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
//using LocatePlate.Infrastructure.Geography;
//using LocatePlate.WebApi.Extensions;

namespace LocatePlate.WebApi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IRestaurantService _restaurantService;
        private readonly IPageService _pageService;
        private readonly LocatePlateContext _locatePlateContext;
        private readonly IHostingEnvironment _environment;

        public RegisterModel(
            UserManager<UserIdentity> userManager,
            SignInManager<UserIdentity> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IRestaurantService restaurantService,
            RoleManager<IdentityRole> roleManager,
            IPageService pageService,
            LocatePlateContext locatePlateContext,
            IHostingEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _restaurantService = restaurantService;
            _roleManager = roleManager;
            _pageService = pageService;
            _locatePlateContext = locatePlateContext;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public List<SelectListItem> Locations { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Business Name")]
            public string BusinessName { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Business Email")]
            public string Email { get; set; }
            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            [MaxLength(10)]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string PhoneNumber { get; set; }
            [Required]
            [Display(Name = "Locality")]
            public string Locality { get; set; }
            [Required]
            [Display(Name = "Postal Code")]
            public string PinCode { get; set; }
            [Required]
            [Display(Name = "Country")]
            public int Country { get; set; } = 38;
            [Required]
            [Display(Name = "Province")]
            public int Province { get; set; } = 9;
            //[Required]
            [Display(Name = "City")]
            public string Location { get; set; }
            [Required]
            [Display(Name = "Business Address")]
            public string BusinessAddress { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Display(Name = "I Agree with the terms and conditions")]
            public bool IsAcceptedTermAndCondition { get; set; }
            public string Signature { get; set; }
            public string Signature64 { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            //GeographyHelper.StateDropdown();
            ReturnUrl = returnUrl;
            await OnGetCitiesAsync(38, 9);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task OnGetCitiesAsync(int countryId, int provinceId)
        {
            var pages = _pageService.GetByCountryAndProvince(countryId, provinceId);
            Locations = new List<SelectListItem>();
            pages.ToList().ForEach(c => Locations.Add(new SelectListItem { Text = c.Name, Value = $"{c.Id}" }));
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Admin/Dashboard");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new UserIdentity { BusinessName = Input.BusinessName.ReplaceSpace('_'), UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.PhoneNumber, BusinessAddress = Input.BusinessAddress, NormalizedUserName = Input.BusinessName, Signature = Input.Signature, Role = UserRoles.ResaurantOwner };

                var IsExistingPhone = _locatePlateContext.UserIdentities.Any(c => c.Role == UserRoles.ResaurantOwner &&
                 c.PhoneNumber == user.PhoneNumber);
                if (IsExistingPhone)
                {
                    ModelState.AddModelError(string.Empty, "Restaurant owner with same phone number already exist");
                    return Page();
                }
                var IsExistingEmail = _locatePlateContext.UserIdentities.Any(c => c.Role == UserRoles.ResaurantOwner &&
                 c.Email == user.Email);
                if (IsExistingEmail)
                {
                    ModelState.AddModelError(string.Empty, "Restaurant owner with same email already exist");
                    return Page();
                }

                if (new Guid(Input.Location) == Guid.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Please select locationId");
                    return Page();
                }

                var IsExistingAddress = _locatePlateContext.UserIdentities.Any(c => c.Role == UserRoles.ResaurantOwner &&
                c.NormalizedUserName == user.NormalizedUserName && c.Locality == user.Locality && c.BusinessAddress == user.BusinessAddress);
                if (IsExistingAddress)
                {
                    ModelState.AddModelError(string.Empty, "Restaurant owner with same address already exist");
                    return Page();
                }
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(UserRoles.ResaurantOwner))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.ResaurantOwner));

                    var idenityResult = await _userManager.AddToRoleAsync(user, UserRoles.ResaurantOwner);

                    _logger.LogInformation("User created a new account with password.");
                    var restuarant = new Restaurant
                    {
                        UserId = new Guid(user.Id),
                        Name = Input.BusinessName,
                        About = string.Empty,
                        Locality = Input.Locality,
                        FullAddress = Input.BusinessAddress,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = user.UserName,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = user.UserName,
                        PinCode = Input.PinCode,
                        Country = Input.Country,
                        Province = Input.Province,
                        LocationId = new Guid(Input.Location), // CityId from the cms
                        ResturantType = ResturantType.NotSelected
                    };

                    restuarant.Url = restuarant.RestaurantUrl;
                    try
                    {
                        restuarant.CityName = _pageService.GetAsync(restuarant.LocationId).GetAwaiter().GetResult().Name;
                        this._restaurantService.Insert(restuarant);
                    }
                    catch (Exception)
                    {
                        await _userManager.DeleteAsync(user);
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl,type=1 },
                        protocol: Request.Scheme);
                       

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    var path = Path.Combine(_environment.WebRootPath, "Signature");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string signaturefilename = Guid.NewGuid().ToString();
                    var bytes = Convert.FromBase64String(Input.Signature64.Replace("image/png;base64,", ""));
                    using (var imageFile = new FileStream(path + "/" + signaturefilename + ".jpg", FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }

                    string html = "<html>" +
                    "<body>" +

                    "<div style=\"height:auto;\">" +
                    "<div style=\"padding: 10px; color: #E91E63;\">" +
                    "<h5 style=\"margin:0;\">Terms and Condition</h5>" +
                    "</div>" +
                    "<div style=\"padding:10px;\">" +
                    "<p>Lorem Ipsum is simply dummy text of the <b>Locate Plate</b> printing and typesetting. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s </p>" +
                    "<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s</p>" +
                    "<h4 style=\"color: #E91E63;\">Signature:</h4>" +
                    "<div style=\"min-height: 100px; border: 1px solid #ddd; width: 500px;\">" +
                    "<div style=\"padding:0 !important; margin:0 !important;width: 100% !important; height: 0 !important; -ms-touch-action: none; touch-action: none;margin-top:-1em !important; margin-bottom:1em !important;\"></div>" +
                    "<img src=\"" + path + "/" + signaturefilename + ".jpg\" style=\"width: 300px; height: 150px; padding: 5px; margin: 0 auto; display: block;\" />" +
                    "<div style=\"padding:0 !important; margin:0 !important;width: 100% !important; height: 0 !important; -ms-touch-action: none; touch-action: none;margin-top:-1.5em !important; margin-bottom:1.5em !important; position: relative;\"></div>" +
                    "</div>" +
                    "</div>" +
                    "</div>" +

                    "</body>" +
                    "</html>";
                    var pdfbyte = PDFHelper.HtmlToBase64Pdf(html);

                    System.IO.File.WriteAllBytes(path + "/" + signaturefilename + ".pdf", pdfbyte);

                    EmailHelper.Email(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                        path + "/" + signaturefilename + ".pdf");

                    AmazonS3FileUpload amazonS3FileUpload = new AmazonS3FileUpload();
                    amazonS3FileUpload.UploadFile(signaturefilename + ".pdf", path + "/" + signaturefilename + ".pdf");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //return RedirectToPage(".RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        return LocalRedirect("/Identity/Account/Login?ReturnUrl=%2Fadmin%2FConfirmation%2FIndex");
                    }
                    else
                    {
                        //var userEntity = this._locatePlateContext.UserIdentities.FirstOrDefault(c => c.Id == user.Id);
                        //userEntity.EmailConfirmed = true;
                        //this._locatePlateContext.UserIdentities.Update(userEntity);
                        //this._locatePlateContext.SaveChanges();
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("/Identity/Account/Login?ReturnUrl=%2Fadmin%2FConfirmation%2FIndex");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form

            return Page();
        }
    }
}
