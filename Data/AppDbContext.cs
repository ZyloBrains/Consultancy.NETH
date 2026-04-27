using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;

namespace Consultancy.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<CourseTeacher> CourseTeachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<ContactInquiry> ContactInquiries { get; set; }
    public DbSet<SiteSetting> SiteSettings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<CourseTeacher>()
            .HasKey(ct => new { ct.CourseId, ct.TeacherId });
            
        modelBuilder.Entity<CourseTeacher>()
            .HasOne(ct => ct.Course)
            .WithMany(c => c.CourseTeachers)
            .HasForeignKey(ct => ct.CourseId);
            
        modelBuilder.Entity<CourseTeacher>()
            .HasOne(ct => ct.Teacher)
            .WithMany(t => t.CourseTeachers)
            .HasForeignKey(ct => ct.TeacherId);
            
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CategoryId);
            
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Country)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}