using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface ICountryService
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(int id);
    Task<Country?> GetBySlugAsync(string slug);
    Task<int> GetTotalCountAsync();
}

public class CountryService : ICountryService
{
    private readonly ApplicationDbContext _context;
    
    public CountryService(ApplicationDbContext context)
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

public async Task<int> GetTotalCountAsync()
{
    return await _context.Countries.CountAsync(c => c.IsActive);
}
}