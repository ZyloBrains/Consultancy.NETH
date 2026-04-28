using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Consultancy.Areas.Admin.Pages.DocumentRequirements;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public DocumentRequirement DocumentRequirement { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        DocumentRequirement.CreatedAt = DateTime.UtcNow;
        _context.DocumentRequirements.Add(DocumentRequirement);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
