using System.ComponentModel.DataAnnotations.Schema;

namespace Consultancy.Models.Entities;

public class CourseTeacher
{
    public int CourseId { get; set; }
    
    [ForeignKey("CourseId")]
    public Course? Course { get; set; }
    
    public int TeacherId { get; set; }
    
    [ForeignKey("TeacherId")]
    public Teacher? Teacher { get; set; }
}