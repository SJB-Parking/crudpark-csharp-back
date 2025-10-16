using CrudPark_Back.Data;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Repositories.Implementations;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.CustomerVehicles)
                .ThenInclude(cv => cv.Vehicle)
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.CustomerVehicles)
                .ThenInclude(cv => cv.Vehicle)
            .Include(c => c.MonthlySubscriptions)
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == email && c.IsActive);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return false;

        customer.IsActive = false;
        customer.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Customers
                .AnyAsync(c => c.Email == email && c.Id != excludeId.Value);
        }
        return await _context.Customers.AnyAsync(c => c.Email == email);
    }
}