using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Areas.Admin.Pages.Students;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public StudentInputModel Input { get; set; } = new();

    public IList<Country> Countries { get; set; } = default!;
    public IList<Course> Courses { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Countries = await _context.Countries.ToListAsync();
        Courses = await _context.Courses.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Countries = await _context.Countries.ToListAsync();
            Courses = await _context.Courses.ToListAsync();
            return Page();
        }

        var student = new Student
        {
            Name = Input.Name,
            Email = Input.Email,
            Phone = Input.Phone,
            Address = Input.Address,
            CountryId = Input.CountryId,
            CourseId = Input.CourseId,
            GoogleDriveFolderUrl = Input.GoogleDriveFolderUrl,
            // Demo mode: use "demo_123" as folder ID to simulate Google Drive
            GoogleDriveFolderId = string.IsNullOrEmpty(Input.GoogleDriveFolderId) ? "demo_123" : Input.GoogleDriveFolderId
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return RedirectToPage("/StudentDocuments/Index");
    }
}

public class StudentInputModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public int CourseId { get; set; }
    public string? GoogleDriveFolderUrl { get; set; }
    public string? GoogleDriveFolderId { get; set; }
}
