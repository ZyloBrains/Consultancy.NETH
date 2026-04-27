using Microsoft.EntityFrameworkCore;
using Consultancy.Models.Entities;
using Consultancy.Data;

namespace Consultancy.Services;

public interface IContactService
{
    Task<IEnumerable<ContactInquiry>> GetAllAsync();
    Task<ContactInquiry> CreateAsync(ContactInquiry inquiry);
}

public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;
    
    public ContactService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ContactInquiry>> GetAllAsync()
    {
        return await _context.ContactInquiries
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<ContactInquiry> CreateAsync(ContactInquiry inquiry)
    {
        inquiry.CreatedAt = DateTime.UtcNow;
        _context.ContactInquiries.Add(inquiry);
        await _context.SaveChangesAsync();
        return inquiry;
    }
}