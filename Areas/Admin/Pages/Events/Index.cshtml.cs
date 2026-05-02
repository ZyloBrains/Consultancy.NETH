using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Data;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.NETH.Areas.Admin.Pages.Events;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Consultancy.Models.Entities.Event> Events { get; set; } = new List<Consultancy.Models.Entities.Event>();

    public async Task OnGetAsync()
    {
        Events = await _context.Events
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }
}
