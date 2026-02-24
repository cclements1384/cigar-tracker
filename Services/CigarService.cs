using cigar_tracker.Data;
using cigar_tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace cigar_tracker.Services;

public class CigarService
{
    private readonly CigarTrackerDbContext _context;

    public CigarService(CigarTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cigar>> GetAllCigarsAsync()
    {
        return await _context.Cigars.OrderByDescending(c => c.DateSmoked).ToListAsync();
    }

    public async Task<List<Cigar>> GetCigarsByUserAsync(string userEmail)
    {
        return await _context.Cigars
            .Where(c => c.LoggedInUser == userEmail)
            .OrderByDescending(c => c.DateSmoked)
            .ToListAsync();
    }

    public async Task AddCigarAsync(Cigar cigar)
    {
        _context.Cigars.Add(cigar);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCigarAsync(int id)
    {
        var cigar = await _context.Cigars.FindAsync(id);
        if (cigar != null)
        {
            _context.Cigars.Remove(cigar);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Cigar?> GetCigarByIdAsync(int id)
    {
        return await _context.Cigars.FirstOrDefaultAsync(c => c.Id == id);
    }
}
