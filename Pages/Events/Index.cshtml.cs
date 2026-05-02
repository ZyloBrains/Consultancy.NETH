using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Services;

namespace Consultancy.NETH.Pages.Events;

public class IndexModel : PageModel
{
    private readonly IEventService _eventService;

    public IndexModel(IEventService eventService)
    {
        _eventService = eventService;
    }

    public IEnumerable<Consultancy.Models.Entities.Event> Events { get; set; } = new List<Consultancy.Models.Entities.Event>();

    public async Task OnGetAsync()
    {
        Events = await _eventService.GetAllAsync();
        ViewData["Title"] = "Events - NETH Educational Consultancy";
    }
}
