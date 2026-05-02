using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Consultancy.NETH.Areas.Admin.Pages.Events;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public EventInputModel Input { get; set; } = new();

    public class EventInputModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TitleNp { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DescriptionNp { get; set; }
        public string? Image { get; set; }
        public string? Location { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsActive { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt == null)
        {
            return NotFound();
        }

        Input = new EventInputModel
        {
            Id = evt.Id,
            Title = evt.Title,
            TitleNp = evt.TitleNp,
            Slug = evt.Slug,
            Description = evt.Description,
            DescriptionNp = evt.DescriptionNp,
            Image = evt.Image,
            Location = evt.Location,
            EventDate = evt.EventDate,
            IsActive = evt.IsActive
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var evt = await _context.Events.FindAsync(Input.Id);
        if (evt == null)
        {
            return NotFound();
        }

        evt.Title = Input.Title;
        evt.TitleNp = Input.TitleNp;
        evt.Slug = Input.Slug.ToLower().Replace(" ", "-");
        evt.Description = Input.Description;
        evt.DescriptionNp = Input.DescriptionNp;
        evt.Image = Input.Image;
        evt.Location = Input.Location;
        evt.EventDate = Input.EventDate;
        evt.IsActive = Input.IsActive;

        await _context.SaveChangesAsync();

        return RedirectToPage("/Events/Index");
    }
}
