using System.ComponentModel.DataAnnotations;

namespace Consultancy.Models.Entities;

public class Teacher
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string NameNp { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Designation { get; set; }

    [MaxLength(100)]
    public string? DesignationNp { get; set; }

    [MaxLength(500)]
    public string? Photo { get; set; }

    public string? Bio { get; set; }

    public string? BioNp { get; set; }

    [MaxLength(200)]
    public string? Facebook { get; set; }

    [MaxLength(200)]
    public string? Instagram { get; set; }

    [MaxLength(200)]
    public string? LinkedIn { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsFeatured { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();
}