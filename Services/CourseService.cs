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
    Task<int> GetTotalCountAsync();
}

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;
    
    public CourseService(ApplicationDbContext context)
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
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
public async Task<Course?> GetBySlugAsync(string slug)
{
    return await _context.Courses
        .Include(c => c.Category)
        .Include(c => c.Country)
        .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);
}

public async Task<int> GetTotalCountAsync()
{
    return await _context.Courses.CountAsync(c => c.IsActive);
}
}