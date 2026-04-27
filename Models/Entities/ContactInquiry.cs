using System.ComponentModel.DataAnnotations;

namespace Consultancy.Models.Entities;

public class ContactInquiry
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
    
    [MaxLength(200)]
    public string? Subject { get; set; }
    
    public string? Message { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "New";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}