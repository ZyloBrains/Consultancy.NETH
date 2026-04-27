using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Models;

namespace Consultancy.Pages.Admin.Identity;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var email = Request.Form["Email"].ToString();
        var password = Request.Form["Password"].ToString();
        var rememberMe = Request.Form["RememberMe"].Count > 0;

        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Redirect("/admin");
        }
        else if (result.IsLockedOut)
        {
            TempData["Error"] = "Account is locked. Please try again later.";
        }
        else
        {
            TempData["Error"] = "Invalid email or password";
        }
        return Page();
    }
}