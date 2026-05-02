using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Services;

namespace Consultancy.NETH.Pages;

public class IndexModel : PageModel
{
    private readonly ICourseService _courseService;
    private readonly ICountryService _countryService;
    private readonly ITeacherService _teacherService;
    private readonly IEventService _eventService;

    public IndexModel(ICourseService courseService, ICountryService countryService, 
                   ITeacherService teacherService, IEventService eventService)
    {
        _courseService = courseService;
        _countryService = countryService;
        _teacherService = teacherService;
        _eventService = eventService;
    }

    public IEnumerable<Consultancy.Models.Entities.Course> Courses { get; set; } = new List<Consultancy.Models.Entities.Course>();
    public IEnumerable<Consultancy.Models.Entities.Country> Countries { get; set; } = new List<Consultancy.Models.Entities.Country>();
    public IEnumerable<Consultancy.Models.Entities.Teacher> Teachers { get; set; } = new List<Consultancy.Models.Entities.Teacher>();
    public IEnumerable<Consultancy.Models.Entities.Event> UpcomingEvents { get; set; } = new List<Consultancy.Models.Entities.Event>();

    public async Task OnGetAsync()
    {
        Courses = await _courseService.GetFeaturedAsync();
        Countries = await _countryService.GetFeaturedAsync(4);
        Teachers = await _teacherService.GetFeaturedAsync();
        UpcomingEvents = await _eventService.GetUpcomingAsync(3);
    }
}
