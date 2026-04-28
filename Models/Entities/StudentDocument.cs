using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultancy.Models.Entities;

public class StudentDocument
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }
    
    public int DocumentRequirementId { get; set; }
    
    [ForeignKey("DocumentRequirementId")]
    public DocumentRequirement? DocumentRequirement { get; set; }
    
    public string Status { get; set; } = "Pending"; // Pending, Submitted, Verified
    
    [MaxLength(500)]
    public string? FileName { get; set; }
    
    [MaxLength(500)]
    public string? GoogleFileId { get; set; }
    
    public DateTime? LastCheckedAt { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
