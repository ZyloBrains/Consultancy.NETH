using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultancy.Models.Entities;

public class Course
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string NameNp { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Slug { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public string? DescriptionNp { get; set; }
    
    [MaxLength(500)]
    public string? Image { get; set; }
    
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    
    public int? CountryId { get; set; }
    
    [ForeignKey("CountryId")]
    public Country? Country { get; set; }
    
    [MaxLength(100)]
    public string? Duration { get; set; }
    
    public decimal? Fees { get; set; }
    
    public bool IsFeatured { get; set; }
    
    public int DisplayOrder { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();
    public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}