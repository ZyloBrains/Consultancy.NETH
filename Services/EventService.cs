using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetUpcomingAsync(int count = 3);
    Task<Event?> GetBySlugAsync(string slug);
    Task<Event?> GetByIdAsync(int id);
    Task<Event> CreateAsync(Event evt);
    Task<Event> UpdateAsync(Event evt);
    Task DeleteAsync(int id);
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

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<Event> CreateAsync(Event evt)
    {
        evt.CreatedAt = DateTime.UtcNow;
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();
        return evt;
    }

    public async Task<Event> UpdateAsync(Event evt)
    {
        _context.Events.Update(evt);
        await _context.SaveChangesAsync();
        return evt;
    }

    public async Task DeleteAsync(int id)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt != null)
        {
            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
        }
    }
}