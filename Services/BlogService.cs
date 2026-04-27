using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface IBlogService
{
    Task<IEnumerable<Blog>> GetAllAsync();
    Task<IEnumerable<Blog>> GetRecentAsync(int count = 3);
    Task<Blog?> GetBySlugAsync(string slug);
}

public class BlogService : IBlogService
{
    private readonly ApplicationDbContext _context;
    
    public BlogService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Blog>> GetAllAsync()
    {
        return await _context.Blogs
            .Where(b => b.IsActive)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Blog>> GetRecentAsync(int count = 3)
    {
        return await _context.Blogs
            .Where(b => b.IsActive)
            .OrderByDescending(b => b.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<Blog?> GetBySlugAsync(string slug)
    {
        return await _context.Blogs
            .FirstOrDefaultAsync(b => b.Slug == slug && b.IsActive);
    }
}