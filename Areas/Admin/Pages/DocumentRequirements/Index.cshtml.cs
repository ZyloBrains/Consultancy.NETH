using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Areas.Admin.Pages.DocumentRequirements;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<DocumentRequirement> DocumentRequirements { get; set; } = default!;

    public async Task OnGetAsync()
    {
        DocumentRequirements = await _context.DocumentRequirements
            .OrderBy(dr => dr.DisplayOrder)
            .ToListAsync();
    }
}
