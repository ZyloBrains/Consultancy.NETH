using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetUpcomingAsync(int count = 3);
    Task<Event?> GetBySlugAsync(string slug);
}

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;
    
    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events
            .Where(e => e.IsActive)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Event>> GetUpcomingAsync(int count = 3)
    {
        return await _context.Events
            .Where(e => e.IsActive && e.EventDate >= DateTime.UtcNow)
            .OrderBy(e => e.EventDate)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<Event?> GetBySlugAsync(string slug)
    {
        return await _context.Events
            .FirstOrDefaultAsync(e => e.Slug == slug && e.IsActive);
    }
}