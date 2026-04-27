using System.ComponentModel.DataAnnotations;

namespace Consultancy.Models.Entities;

public class SiteSetting
{
    [Key]
    [MaxLength(100)]
    public string Key { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Value { get; set; }
    
    [MaxLength(1000)]
    public string? ValueNp { get; set; }
    
    [MaxLength(100)]
    public string? Category { get; set; }
}