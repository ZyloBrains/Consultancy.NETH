using Consultancy.Data;
using Consultancy.Models.Entities;
using Consultancy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Consultancy.Areas.Admin.Pages.StudentDocuments;

public class VerifyModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IDocumentVerificationService _verificationService;

    public VerifyModel(ApplicationDbContext context, IDocumentVerificationService verificationService)
    {
        _context = context;
        _verificationService = verificationService;
    }

    [BindProperty(SupportsGet = true)]
    public int StudentId { get; set; }

    public Student? Student { get; set; }
    public List<DocumentChecklistItem> Checklist { get; set; } = new();
    public int TotalDocuments { get; set; }
    public int SubmittedCount { get; set; }
    public int CompletionPercentage { get; set; }
    public DateTime? LastVerifiedAt { get; set; }
    public List<DocumentRequirement> AvailableRequirements { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int studentid)
    {
        StudentId = studentid;
        Student = await _context.Students.FindAsync(StudentId);
        if (Student == null)
        {
            return NotFound();
        }

        await LoadChecklist();
        return Page();
    }

    public async Task<IActionResult> OnPostVerifyAsync(int studentid)
    {
        StudentId = studentid;
        Student = await _context.Students.FindAsync(StudentId);
        if (Student == null)
        {
            return NotFound();
        }

        await _verificationService.VerifyStudentDocumentsAsync(StudentId);
        await LoadChecklist();
        return Page();
    }

    public async Task<IActionResult> OnPostUploadAsync(int studentid, IFormFile file, int documentRequirementId)
    {
        StudentId = studentid;
        Student = await _context.Students.FindAsync(StudentId);
        if (Student == null)
        {
            return NotFound();
        }

        if (file != null && file.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", StudentId.ToString());
            Directory.CreateDirectory(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, file.FileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var existingDoc = await _context.StudentDocuments
                .FirstOrDefaultAsync(d => d.StudentId == StudentId && d.DocumentRequirementId == documentRequirementId);

            if (existingDoc == null)
            {
                _context.StudentDocuments.Add(new StudentDocument
                {
                    StudentId = StudentId,
                    DocumentRequirementId = documentRequirementId,
                    Status = "Submitted",
                    FileName = file.FileName,
                    CreatedAt = DateTime.UtcNow,
                    LastCheckedAt = DateTime.UtcNow
                });
            }
            else
            {
                existingDoc.Status = "Submitted";
                existingDoc.FileName = file.FileName;
                existingDoc.LastCheckedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        await LoadChecklist();
        return Page();
    }

    private async Task LoadChecklist()
    {
        var result = await _verificationService.GetStudentChecklistAsync(StudentId);
        if (result.Success)
        {
            Checklist = result.Checklist;
            TotalDocuments = Checklist.Count;
            SubmittedCount = Checklist.Count(c => c.Status == "Submitted" || c.Status == "Verified");
            CompletionPercentage = TotalDocuments > 0 ? (int)Math.Round((double)SubmittedCount / TotalDocuments * 100) : 0;
            LastVerifiedAt = result.LastVerifiedAt;
        }

        AvailableRequirements = await _context.DocumentRequirements
            .Where(r => r.IsActive)
            .OrderBy(r => r.DisplayOrder)
            .ToListAsync();
    }
}
