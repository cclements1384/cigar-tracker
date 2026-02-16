namespace cigar_tracker.Models;

public class UpcProduct
{
    public string Upc { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
    public string? Retailer { get; set; }
    public bool Found { get; set; }
}
