using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;

namespace Consultancy.Data;

public class ApplicationDbContext : IdentityDbContext<Models.ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CourseTeacher>()
            .HasKey(ct => new { ct.CourseId, ct.TeacherId });
            
        builder.Entity<CourseTeacher>()
            .HasOne(ct => ct.Course)
            .WithMany(c => c.CourseTeachers)
            .HasForeignKey(ct => ct.CourseId);
            
        builder.Entity<CourseTeacher>()
            .HasOne(ct => ct.Teacher)
            .WithMany(t => t.CourseTeachers)
            .HasForeignKey(ct => ct.TeacherId);
            
        builder.Entity<Course>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CategoryId);
            
        builder.Entity<Course>()
            .HasOne(c => c.Country)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Rename tables to remove AspNet prefix
        builder.Entity<Models.ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
        });

        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
        });

        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("Roles");
        });

        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.HasKey(rc => rc.Id);
            entity.ToTable("RoleClaims");
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            entity.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        builder.Entity<IdentityUserToken<string>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }
}