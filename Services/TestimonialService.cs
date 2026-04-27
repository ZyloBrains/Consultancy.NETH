using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface ITestimonialService
{
    Task<IEnumerable<Testimonial>> GetRecentAsync(int count = 3);
}

public class TestimonialService : ITestimonialService
{
    private readonly ApplicationDbContext _context;
    
    public TestimonialService(ApplicationDbContext context)
    {
        _context = context;
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
}