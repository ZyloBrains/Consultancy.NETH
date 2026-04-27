using System.ComponentModel.DataAnnotations;

namespace Consultancy.Models.Entities;

public class Country
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string NameNp { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? FlagImage { get; set; }
    
    public string? Description { get; set; }
    
    public string? DescriptionNp { get; set; }
    
    [MaxLength(500)]
    public string? Universities { get; set; }
    
    [MaxLength(500)]
    public string? CostOfLiving { get; set; }
    
    [MaxLength(1000)]
    public string? VisaInfo { get; set; }
    
    [MaxLength(1000)]
    public string? WorkPermit { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int DisplayOrder { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}