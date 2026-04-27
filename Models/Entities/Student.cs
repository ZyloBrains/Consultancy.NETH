using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultancy.Models.Entities;

public class Student
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    public int? CourseId { get; set; }
    
    [ForeignKey("CourseId")]
    public Course? Course { get; set; }
    
    public int? CountryId { get; set; }
    
    [ForeignKey("CountryId")]
    public Country? Country { get; set; }
    
    public string? Message { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "New";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}