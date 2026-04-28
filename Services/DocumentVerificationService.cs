using Consultancy.Data;
using Consultancy.Models.Entities;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Consultancy.Services;

public interface IDocumentVerificationService
{
    Task<DocumentChecklistResult> GetStudentChecklistAsync(int studentId);
    Task<DocumentChecklistResult> VerifyStudentDocumentsAsync(int studentId);
    Task SeedDocumentRequirementsAsync();
}

public class DocumentVerificationService : IDocumentVerificationService
{
    private readonly ApplicationDbContext _context;
    private readonly IGoogleDriveService _googleDriveService;
    
    public DocumentVerificationService(ApplicationDbContext context, IGoogleDriveService googleDriveService)
    {
        _context = context;
        _googleDriveService = googleDriveService;
    }
    
    public async Task<DocumentChecklistResult> GetStudentChecklistAsync(int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            return new DocumentChecklistResult { Success = false, Message = "Student not found" };
        }
        
        var requirements = await _context.DocumentRequirements
            .Where(dr => dr.IsActive)
            .OrderBy(dr => dr.DisplayOrder)
            .ToListAsync();
        
        var studentDocs = await _context.StudentDocuments
            .Where(sd => sd.StudentId == studentId)
            .Include(sd => sd.DocumentRequirement)
            .ToListAsync();
        
        var checklist = new List<DocumentChecklistItem>();
        
        foreach (var req in requirements)
        {
            var doc = studentDocs.FirstOrDefault(sd => sd.DocumentRequirementId == req.Id);
            checklist.Add(new DocumentChecklistItem
            {
                DocumentRequirementId = req.Id,
                DocumentName = req.Name,
                Description = req.Description,
                Status = doc?.Status ?? "Pending",
                FileName = doc?.FileName,
                LastCheckedAt = doc?.LastCheckedAt
            });
        }
        
        return new DocumentChecklistResult
        {
            Success = true,
            StudentId = student.Id,
            StudentName = $"{student.Name}",
            GoogleDriveFolderUrl = student.GoogleDriveFolderUrl,
            Checklist = checklist,
            LastVerifiedAt = studentDocs.Max(sd => sd.LastCheckedAt)
        };
    }
    
    public async Task<DocumentChecklistResult> VerifyStudentDocumentsAsync(int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            return new DocumentChecklistResult { Success = false, Message = "Student not found" };
        }
        
        if (string.IsNullOrEmpty(student.GoogleDriveFolderId))
        {
            return new DocumentChecklistResult { Success = false, Message = "Google Drive folder not configured for this student" };
        }
        
        // Get required documents
        var requirements = await _context.DocumentRequirements
            .Where(dr => dr.IsActive)
            .OrderBy(dr => dr.DisplayOrder)
            .ToListAsync();
        
        // Get files from Google Drive
        var driveFiles = await _googleDriveService.ListFilesInFolderAsync(student.GoogleDriveFolderId);
        
        // Clear existing records for this student
        var existingDocs = await _context.StudentDocuments
            .Where(sd => sd.StudentId == studentId)
            .ToListAsync();
        _context.StudentDocuments.RemoveRange(existingDocs);
        
        // Match files to requirements
        foreach (var req in requirements)
        {
            var matchedFile = FindMatchingFile(driveFiles, req.Name);
            
            var studentDoc = new StudentDocument
            {
                StudentId = studentId,
                DocumentRequirementId = req.Id,
                Status = matchedFile != null ? "Submitted" : "Pending",
                FileName = matchedFile?.Name,
                GoogleFileId = matchedFile?.Id,
                LastCheckedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.StudentDocuments.Add(studentDoc);
        }
        
        await _context.SaveChangesAsync();
        
        return await GetStudentChecklistAsync(studentId);
    }
    
    public async Task SeedDocumentRequirementsAsync()
    {
        var defaultDocs = new[]
        {
            ("Transcript", "Academic transcript from previous institution", 1),
            ("Passport", "Valid passport document", 2),
            ("Certificates", "Relevant certificates (birth, marriage, etc.) ", 3),
            ("Photo ID", "Recent passport-sized photograph", 4),
            ("English Proficiency", "IELTS/TOEFL/PTE scores", 5)
        };
        
        foreach (var (name, desc, order) in defaultDocs)
        {
            if (!await _context.DocumentRequirements.AnyAsync(dr => dr.Name == name))
            {
                _context.DocumentRequirements.Add(new DocumentRequirement
                {
                    Name = name,
                    Description = desc,
                    DisplayOrder = order,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        
        await _context.SaveChangesAsync();
    }
    
    private GoogleFile? FindMatchingFile(List<GoogleFile> files, string requirementName)
    {
        // Try exact match first
        var exactMatch = files.FirstOrDefault(f => 
            f.Name.Contains(requirementName, StringComparison.OrdinalIgnoreCase));
        
        if (exactMatch != null)
            return exactMatch;
        
        // Try keyword matching
        var keywords = requirementName.ToLower().Split(' ', '-', '_');
        foreach (var file in files)
        {
            var fileName = file.Name.ToLower();
            var matchCount = keywords.Count(k => fileName.Contains(k));
            if (matchCount >= keywords.Length / 2.0) // At least half of keywords match
            {
                return file;
            }
        }
        
        return null;
    }
}

public class DocumentChecklistResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? GoogleDriveFolderUrl { get; set; }
    public DateTime? LastVerifiedAt { get; set; }
    public List<DocumentChecklistItem> Checklist { get; set; } = new();
}

public class DocumentChecklistItem
{
    public int DocumentRequirementId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Submitted, Verified
    public string? FileName { get; set; }
    public DateTime? LastCheckedAt { get; set; }
}
