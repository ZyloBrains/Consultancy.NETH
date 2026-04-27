using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Models;

namespace Consultancy.Pages.Admin.Identity;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var firstName = Request.Form["FirstName"].ToString();
        var lastName = Request.Form["LastName"].ToString();
        var email = Request.Form["Email"].ToString();
        var password = Request.Form["Password"].ToString();
        var confirmPassword = Request.Form["ConfirmPassword"].ToString();

        if (password != confirmPassword)
        {
            TempData["Error"] = "Passwords do not match";
            return Page();
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            TempData["Success"] = "true";
        }
        else
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            TempData["Error"] = errors;
        }
        return Page();
    }
}