using Microsoft.AspNetCore.Mvc.RazorPages;
using Consultancy.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Consultancy.NETH.Areas.Admin.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalCountries { get; set; }
        public int TotalTeachers { get; set; }
        public int NewInquiries { get; set; }
        public int PendingDocuments { get; set; }

        public string StudentsChartData { get; set; } = "[]";
        public string CourseChartLabels { get; set; } = "[]";
        public string CourseChartData { get; set; } = "[]";

        public List<RecentStudentViewModel> RecentStudents { get; set; } = new();
        public List<RecentInquiryViewModel> RecentInquiries { get; set; } = new();

        public async Task OnGet()
        {
            TotalStudents = await _context.Students.CountAsync();
            TotalCourses = await _context.Courses.CountAsync(c => c.IsActive);
            TotalCountries = await _context.Countries.CountAsync(c => c.IsActive);
            TotalTeachers = await _context.Teachers.CountAsync(t => t.IsActive);
            NewInquiries = await _context.ContactInquiries.CountAsync(c => c.Status == "New");
            PendingDocuments = await _context.StudentDocuments.CountAsync(d => d.Status == "Pending");

            var monthlyData = new List<int>();
            for (int i = 5; i >= 0; i--)
            {
                var count = await _context.Students.CountAsync(s => s.Id > 0);
                monthlyData.Add(count);
            }
            StudentsChartData = JsonSerializer.Serialize(monthlyData);

            var courseGroups = await _context.Courses
                .Include(c => c.Category)
                .Where(c => c.IsActive)
                .GroupBy(c => c.Category!.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToListAsync();

            CourseChartLabels = JsonSerializer.Serialize(courseGroups.Select(x => x.Name));
            CourseChartData = JsonSerializer.Serialize(courseGroups.Select(x => x.Count));

            RecentStudents = await _context.Students
                .Include(s => s.Country)
                .Include(s => s.Course)
                .OrderByDescending(s => s.Id)
                .Take(5)
                .Select(s => new RecentStudentViewModel
                {
                    Name = s.Name,
                    CourseName = s.Course!.Name,
                    CountryName = s.Country!.Name,
                    Email = s.Email
                })
                .ToListAsync();

            RecentInquiries = await _context.ContactInquiries
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(c => new RecentInquiryViewModel
                {
                    Name = c.Name,
                    Subject = c.Subject,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public class RecentStudentViewModel
        {
            public string Name { get; set; } = string.Empty;
            public string CourseName { get; set; } = string.Empty;
            public string CountryName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        public class RecentInquiryViewModel
        {
            public string Name { get; set; } = string.Empty;
            public string Subject { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }
    }
}
