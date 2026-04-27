using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface ITeacherService
{
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<IEnumerable<Teacher>> GetFeaturedAsync();
    Task<Teacher?> GetByIdAsync(int id);
    Task<Teacher> CreateAsync(Teacher teacher);
    Task UpdateAsync(Teacher teacher);
    Task DeleteAsync(int id);
}

public class TeacherService : ITeacherService
{
    private readonly AppDbContext _context;
    
    public TeacherService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Teacher>> GetAllAsync()
    {
        return await _context.Teachers
            .Where(t => t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Teacher>> GetFeaturedAsync()
    {
        return await _context.Teachers
            .Where(t => t.IsFeatured && t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
    }
    
    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await _context.Teachers
            .Include(t => t.CourseTeachers)
            .ThenInclude(ct => ct.Course)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<Teacher> CreateAsync(Teacher teacher)
    {
        teacher.CreatedAt = DateTime.UtcNow;
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        return teacher;
    }
    
    public async Task UpdateAsync(Teacher teacher)
    {
        _context.Teachers.Update(teacher);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null)
        {
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
    }
}