using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface ITeacherService
{
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<IEnumerable<Teacher>> GetFeaturedAsync();
    Task<int> GetTotalCountAsync();
}

public class TeacherService : ITeacherService
{
    private readonly ApplicationDbContext _context;
    
    public TeacherService(ApplicationDbContext context)
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

public async Task<int> GetTotalCountAsync()
{
    return await _context.Teachers.CountAsync(t => t.IsActive);
}
}