using Consultancy.Data;
using Consultancy.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Consultancy.Areas.Admin.Pages.StudentDocuments;

public class ManageModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ManageModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public int StudentId { get; set; }

    public Student Student { get; set; } = default!;
    public IList<DocumentRequirement> Requirements { get; set; } = default!;
    public IList<StudentDocument> Documents { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        Student = await _context.Students.FindAsync(StudentId);
        if (Student == null) return NotFound();

        Requirements = await _context.DocumentRequirements
            .Where(r => r.IsActive)
            .OrderBy(r => r.DisplayOrder)
            .ToListAsync();

        Documents = await _context.StudentDocuments
            .Where(d => d.StudentId == StudentId)
            .Include(d => d.DocumentRequirement)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int documentId, string status)
    {
        var doc = await _context.StudentDocuments.FindAsync(documentId);
        if (doc == null) return NotFound();

        doc.Status = status;
        doc.LastCheckedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return RedirectToPage("Manage", new { studentId = doc.StudentId });
    }

    public async Task<IActionResult> OnPostAddAsync(int studentId, int requirementId, string fileName)
    {
        var exists = await _context.StudentDocuments
            .AnyAsync(d => d.StudentId == studentId && d.DocumentRequirementId == requirementId);

        if (!exists)
        {
            _context.StudentDocuments.Add(new StudentDocument
            {
                StudentId = studentId,
                DocumentRequirementId = requirementId,
                Status = "Submitted",
                FileName = fileName,
                CreatedAt = DateTime.UtcNow,
                LastCheckedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("Manage", new { studentId });
    }
}
