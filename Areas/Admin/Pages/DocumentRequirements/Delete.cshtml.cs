using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Consultancy.Areas.Admin.Pages.DocumentRequirements;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public DocumentRequirement DocumentRequirement { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var docReq = await _context.DocumentRequirements.FindAsync(id);
        if (docReq == null)
        {
            return NotFound();
        }
        DocumentRequirement = docReq;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var docReq = await _context.DocumentRequirements.FindAsync(DocumentRequirement.Id);
        if (docReq != null)
        {
            _context.DocumentRequirements.Remove(docReq);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
