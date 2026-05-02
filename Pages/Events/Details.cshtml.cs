using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Consultancy.Services;

namespace Consultancy.NETH.Pages.Events;

public class DetailsModel : PageModel
{
    private readonly IEventService _eventService;

    public DetailsModel(IEventService eventService)
    {
        _eventService = eventService;
    }

    public Consultancy.Models.Entities.Event? Event { get; set; }

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return RedirectToPage("/Events/Index");
        }

        Event = await _eventService.GetBySlugAsync(slug);

        if (Event == null)
        {
            return NotFound();
        }

        ViewData["Title"] = Event.Title + " - NETH Educational Consultancy";
        return Page();
    }
}
