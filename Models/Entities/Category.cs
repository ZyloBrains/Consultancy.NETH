using System.ComponentModel.DataAnnotations;

namespace Consultancy.Models.Entities;

public class Category
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
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}