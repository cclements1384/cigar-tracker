namespace cigar_tracker.Models;

public class Cigar
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-10 scale
    public DateTime DateSmoked { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string LoggedInUser { get; set; } = string.Empty; // Email of user who logged this cigar
    public string? ImageUrl { get; set; } // Azure Blob Storage URI
    public string? ImageFileName { get; set; } // Original filename
    public DateTime? ImageUploadedAt { get; set; } // When image was uploaded
}
