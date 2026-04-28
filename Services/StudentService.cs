using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Student>> GetRecentAsync(int count);
}

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    
    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students
            .Include(s => s.Country)
            .Include(s => s.Course)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }
    
    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Country)
            .Include(s => s.Course)
            .Include(s => s.Documents)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Students.CountAsync();
    }
    
    public async Task<IEnumerable<Student>> GetRecentAsync(int count)
    {
        return await _context.Students
            .Include(s => s.Country)
            .Include(s => s.Course)
            .OrderByDescending(s => s.Id)
            .Take(count)
            .ToListAsync();
    }
}
