using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultancy.Models.Entities;

public class Testimonial
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string StudentNameNp { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? StudentPhoto { get; set; }
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    public string? MessageNp { get; set; }
    
    public int? CourseId { get; set; }
    
    [ForeignKey("CourseId")]
    public Course? Course { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int DisplayOrder { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}