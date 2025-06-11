using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyTasks.Areas.Identity.Models;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MyTasks.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel(RegisterParams registerParams) : PageModel
    {
        readonly SignInManager<ApplicationUser> _signInManager = registerParams.SignInManager;
        readonly UserManager<ApplicationUser> _userManager = registerParams.UserManager;
        readonly ICategoriesService _categoriesService = registerParams.CategoriesService;
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress(ErrorMessage = "Podany email jest nieprawidłowy.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} musi mieć od {2} do {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie różnią się.")]
            public string ConfirmPassword { get; set; }
        }

        public async System.Threading.Tasks.Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = [.. (await _signInManager.GetExternalAuthenticationSchemesAsync())];
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = [.. (await _signInManager.GetExternalAuthenticationSchemesAsync())];
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.Info("Użytkownik stworzył nowe hasło.");

                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    Category defaultCategory = new()
                    {
                        Name = "Ogólna",
                        UserId = user.Id
                    };
                    await _categoriesService.Add(defaultCategory);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                        ModelState.AddModelError("Input.Email", "Ten adres email jest już zajęty.");
                    else
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
