using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Areas.Admin.Pages.DocumentRequirements;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
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
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(DocumentRequirement).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DocumentRequirementExists(DocumentRequirement.Id))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool DocumentRequirementExists(int id)
    {
        return _context.DocumentRequirements.Any(e => e.Id == id);
    }
}
