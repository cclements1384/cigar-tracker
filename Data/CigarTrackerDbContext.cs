using cigar_tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace cigar_tracker.Data;

public class CigarTrackerDbContext : DbContext
{
    public CigarTrackerDbContext(DbContextOptions<CigarTrackerDbContext> options) : base(options)
    {
    }

    public DbSet<Cigar> Cigars { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cigar>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Brand).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Size).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.DateSmoked).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.LoggedInUser).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(2048);
            entity.Property(e => e.ImageFileName).HasMaxLength(255);
            entity.Property(e => e.ImageUploadedAt);
        });
    }
}
