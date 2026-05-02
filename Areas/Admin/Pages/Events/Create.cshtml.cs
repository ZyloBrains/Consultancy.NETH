using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Consultancy.NETH.Areas.Admin.Pages.Events;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public EventInputModel Input { get; set; } = new();

    public class EventInputModel
    {
        public string Title { get; set; } = string.Empty;
        public string TitleNp { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DescriptionNp { get; set; }
        public string? Image { get; set; }
        public string? Location { get; set; }
        public DateTime EventDate { get; set; } = DateTime.Now.AddDays(7);
        public bool IsActive { get; set; } = true;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var evt = new Event
        {
            Title = Input.Title,
            TitleNp = Input.TitleNp,
            Slug = Input.Slug.ToLower().Replace(" ", "-"),
            Description = Input.Description,
            DescriptionNp = Input.DescriptionNp,
            Image = Input.Image,
            Location = Input.Location,
            EventDate = Input.EventDate,
            IsActive = Input.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Events.Add(evt);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Events/Index");
    }
}
