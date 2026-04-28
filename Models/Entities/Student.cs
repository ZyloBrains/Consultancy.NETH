using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultancy.Models.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public Country? Country { get; set; }
    public int CourseId { get; set; }
    public Course? Course { get; set; }
    public string? GoogleDriveFolderId { get; set; }
    public string? GoogleDriveFolderUrl { get; set; }
    
    [InverseProperty("Student")]
    public ICollection<StudentDocument> Documents { get; set; } = new List<StudentDocument>();
}