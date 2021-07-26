using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyForum.Areas.Identity.reCaptcha;
using MyForum.Core;

namespace MyForum.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MyUser> _signInManager;
        private readonly UserManager<MyUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IConfiguration _config;
        private readonly GoogleRecaptchaResult reCaptchaResult;

        public RegisterModel(
            UserManager<MyUser> userManager,
            SignInManager<MyUser> signInManager,
            ILogger<RegisterModel> logger,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
            reCaptchaResult = new GoogleRecaptchaResult();
            CaptchaOn = _config.GetValue<bool>("MySettings:CaptchaOn");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public string GoogleReCaptchaKey { get; set; }

        public bool CaptchaOn { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            GoogleReCaptchaKey = _config["GoogleReCaptcha:key"];
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                Rank defaultRank;
                Enum.TryParse( _config.GetValue<string>("MySettings:DefaultRank"), out defaultRank);

                var user = new MyUser
                {
                    UserName = Input.Username,
                    RegistrationDate = DateTime.Today,
                    PrestigePoints = 0,
                    Rank = defaultRank
                };

                if(CaptchaOn)
                {
                    string EncodedResponse = Request.Form["g-Recaptcha-Response"];
                    string secret = _config["GoogleReCaptcha:secret"];
                    bool isCaptchaValid = (reCaptchaResult.Validate(EncodedResponse, secret) == "true" ? true : false);

                    if (!isCaptchaValid)    //Captcha check
                    {
                        ModelState.AddModelError(string.Empty, "You failed the CAPTCHA");
                        GoogleReCaptchaKey = _config["GoogleReCaptcha:key"];
                        return Page();
                    }
                }
                
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            GoogleReCaptchaKey = _config["GoogleReCaptcha:key"];
            return Page();
        }
    }
}
