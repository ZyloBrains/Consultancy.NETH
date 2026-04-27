using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<IEnumerable<Course>> GetFeaturedAsync();
    Task<Course?> GetByIdAsync(int id);
    Task<Course?> GetBySlugAsync(string slug);
    Task<Course> CreateAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(int id);
}

public class CourseService : ICourseService
{
    private readonly AppDbContext _context;
    
    public CourseService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Country)
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Course>> GetFeaturedAsync()
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Country)
            .Where(c => c.IsFeatured && c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }
    
    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Country)
            .Include(c => c.CourseTeachers)
            .ThenInclude(ct => ct.Teacher)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Course?> GetBySlugAsync(string slug)
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Country)
            .Include(c => c.CourseTeachers)
            .ThenInclude(ct => ct.Teacher)
            .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);
    }
    
    public async Task<Course> CreateAsync(Course course)
    {
        course.CreatedAt = DateTime.UtcNow;
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }
    
    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}