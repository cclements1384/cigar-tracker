using cigar_tracker.Data;
using cigar_tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace cigar_tracker.Services;

public class CigarService
{
    private readonly CigarTrackerDbContext _context;
    private readonly AzureStorageService _azureStorageService;

    public CigarService(CigarTrackerDbContext context, AzureStorageService azureStorageService)
    {
        _context = context;
        _azureStorageService = azureStorageService;
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

    public async Task AddCigarWithImageAsync(Cigar cigar, Stream? imageStream, string? imageFileName, string userEmail)
    {
        // Upload image if provided
        if (imageStream != null && !string.IsNullOrEmpty(imageFileName))
        {
            try
            {
                cigar.ImageUrl = await _azureStorageService.UploadImageAsync(imageStream, imageFileName, userEmail);
                cigar.ImageFileName = imageFileName;
                cigar.ImageUploadedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to upload image: {ex.Message}", ex);
            }
        }

        _context.Cigars.Add(cigar);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCigarWithImageAsync(int id)
    {
        var cigar = await _context.Cigars.FindAsync(id);
        if (cigar != null)
        {
            // Delete image from Azure Storage if it exists
            if (!string.IsNullOrEmpty(cigar.ImageUrl))
            {
                await _azureStorageService.DeleteImageAsync(cigar.ImageUrl);
            }

            _context.Cigars.Remove(cigar);
            await _context.SaveChangesAsync();
        }
    }
}
