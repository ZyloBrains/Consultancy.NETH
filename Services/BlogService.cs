using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface IBlogService
{
    Task<IEnumerable<Blog>> GetAllAsync();
    Task<IEnumerable<Blog>> GetRecentAsync(int count = 3);
    Task<Blog?> GetByIdAsync(int id);
    Task<Blog?> GetBySlugAsync(string slug);
    Task<Blog> CreateAsync(Blog blog);
    Task UpdateAsync(Blog blog);
    Task DeleteAsync(int id);
}

public class BlogService : IBlogService
{
    private readonly AppDbContext _context;
    
    public BlogService(AppDbContext context)
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
    
    public async Task<Blog?> GetByIdAsync(int id)
    {
        return await _context.Blogs.FindAsync(id);
    }
    
    public async Task<Blog?> GetBySlugAsync(string slug)
    {
        return await _context.Blogs
            .FirstOrDefaultAsync(b => b.Slug == slug && b.IsActive);
    }
    
    public async Task<Blog> CreateAsync(Blog blog)
    {
        blog.CreatedAt = DateTime.UtcNow;
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();
        return blog;
    }
    
    public async Task UpdateAsync(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog != null)
        {
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
    }
}