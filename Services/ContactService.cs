using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface IContactService
{
    Task<IEnumerable<ContactInquiry>> GetAllAsync();
    Task<IEnumerable<ContactInquiry>> GetUnreadAsync();
    Task<ContactInquiry?> GetByIdAsync(int id);
    Task<ContactInquiry> CreateAsync(ContactInquiry inquiry);
    Task UpdateStatusAsync(int id, string status);
    Task DeleteAsync(int id);
    Task<int> GetUnreadCountAsync();
}

public class ContactService : IContactService
{
    private readonly AppDbContext _context;
    
    public ContactService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ContactInquiry>> GetAllAsync()
    {
        return await _context.ContactInquiries
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<ContactInquiry>> GetUnreadAsync()
    {
        return await _context.ContactInquiries
            .Where(c => c.Status == "New")
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<ContactInquiry?> GetByIdAsync(int id)
    {
        return await _context.ContactInquiries.FindAsync(id);
    }
    
    public async Task<ContactInquiry> CreateAsync(ContactInquiry inquiry)
    {
        inquiry.CreatedAt = DateTime.UtcNow;
        _context.ContactInquiries.Add(inquiry);
        await _context.SaveChangesAsync();
        return inquiry;
    }
    
    public async Task UpdateStatusAsync(int id, string status)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry != null)
        {
            inquiry.Status = status;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task DeleteAsync(int id)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(id);
        if (inquiry != null)
        {
            _context.ContactInquiries.Remove(inquiry);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<int> GetUnreadCountAsync()
    {
        return await _context.ContactInquiries
            .CountAsync(c => c.Status == "New");
    }
}