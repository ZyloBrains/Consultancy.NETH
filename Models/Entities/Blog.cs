using System.ComponentModel.DataAnnotations;

namespace Consultancy.Models.Entities;

public class Blog
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(300)]
    public string TitleNp { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(300)]
    public string Slug { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ShortDescription { get; set; }
    
    [MaxLength(500)]
    public string? ShortDescriptionNp { get; set; }
    
    public string? Content { get; set; }
    
    public string? ContentNp { get; set; }
    
    [MaxLength(500)]
    public string? Image { get; set; }
    
    [MaxLength(200)]
    public string? Author { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}