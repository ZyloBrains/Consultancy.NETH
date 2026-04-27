using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface ITestimonialService
{
    Task<IEnumerable<Testimonial>> GetAllAsync();
    Task<IEnumerable<Testimonial>> GetRecentAsync(int count = 3);
    Task<Testimonial?> GetByIdAsync(int id);
    Task<Testimonial> CreateAsync(Testimonial testimonial);
    Task UpdateAsync(Testimonial testimonial);
    Task DeleteAsync(int id);
}

public class TestimonialService : ITestimonialService
{
    private readonly AppDbContext _context;
    
    public TestimonialService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Testimonial>> GetAllAsync()
    {
        return await _context.Testimonials
            .Include(t => t.Course)
            .Where(t => t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Testimonial>> GetRecentAsync(int count = 3)
    {
        return await _context.Testimonials
            .Include(t => t.Course)
            .Where(t => t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<Testimonial?> GetByIdAsync(int id)
    {
        return await _context.Testimonials
            .Include(t => t.Course)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<Testimonial> CreateAsync(Testimonial testimonial)
    {
        testimonial.CreatedAt = DateTime.UtcNow;
        _context.Testimonials.Add(testimonial);
        await _context.SaveChangesAsync();
        return testimonial;
    }
    
    public async Task UpdateAsync(Testimonial testimonial)
    {
        _context.Testimonials.Update(testimonial);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var testimonial = await _context.Testimonials.FindAsync(id);
        if (testimonial != null)
        {
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
        }
    }
}