using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Areas.Admin.Pages.StudentDocuments;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<StudentDocumentSummary> StudentSummaries { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var students = await _context.Students
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.GoogleDriveFolderUrl,
                Documents = _context.StudentDocuments.Where(d => d.StudentId == s.Id).ToList()
            })
            .OrderBy(s => s.Name)
            .ToListAsync();

        StudentSummaries = students.Select(s => new StudentDocumentSummary
        {
            StudentId = s.Id,
            StudentName = s.Name,
            GoogleDriveFolderUrl = s.GoogleDriveFolderUrl,
            TotalDocuments = s.Documents.Count,
            SubmittedCount = s.Documents.Count(d => d.Status == "Submitted"),
            VerifiedCount = s.Documents.Count(d => d.Status == "Verified"),
            LastCheckedAt = s.Documents.Max(d => d.LastCheckedAt)
        }).ToList();
    }
}

public class StudentDocumentSummary
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string? GoogleDriveFolderUrl { get; set; }
    public int TotalDocuments { get; set; }
    public int SubmittedCount { get; set; }
    public int VerifiedCount { get; set; }
    public DateTime? LastCheckedAt { get; set; }
}
