using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface ICountryService
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(int id);
    Task<Country?> GetBySlugAsync(string slug);
    Task<Country> CreateAsync(Country country);
    Task UpdateAsync(Country country);
    Task DeleteAsync(int id);
}

public class CountryService : ICountryService
{
    private readonly AppDbContext _context;
    
    public CountryService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        return await _context.Countries
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }
    
    public async Task<Country?> GetByIdAsync(int id)
    {
        return await _context.Countries
            .Include(c => c.Courses)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Country?> GetBySlugAsync(string slug)
    {
        return await _context.Countries
            .Include(c => c.Courses)
            .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);
    }
    
    public async Task<Country> CreateAsync(Country country)
    {
        country.CreatedAt = DateTime.UtcNow;
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();
        return country;
    }
    
    public async Task UpdateAsync(Country country)
    {
        _context.Countries.Update(country);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country != null)
        {
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
        }
    }
}