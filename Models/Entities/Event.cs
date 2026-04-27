using System.ComponentModel.DataAnnotations;

namespace Consultancy.Models.Entities;

public class Event
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
    
    public string? Description { get; set; }
    
    public string? DescriptionNp { get; set; }
    
    [MaxLength(500)]
    public string? Image { get; set; }
    
    [MaxLength(300)]
    public string? Location { get; set; }
    
    public DateTime EventDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}