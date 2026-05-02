using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Consultancy.NETH.Areas.Admin.Pages.Events;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public EventDeleteModel Input { get; set; } = new();

    public class EventDeleteModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt == null)
        {
            return NotFound();
        }

        Input = new EventDeleteModel
        {
            Id = evt.Id,
            Title = evt.Title,
            EventDate = evt.EventDate,
            Location = evt.Location,
            IsActive = evt.IsActive
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var evt = await _context.Events.FindAsync(Input.Id);
        if (evt != null)
        {
            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("/Events/Index");
    }
}
