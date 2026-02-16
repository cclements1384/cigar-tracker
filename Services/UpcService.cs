using cigar_tracker.Models;
using System.Net.Http.Json;

namespace cigar_tracker.Services;

public class UpcService
{
    private readonly HttpClient _httpClient;
    private const string UPC_API_URL = "https://api.upcitemdb.com/prod/trial/lookup";

    public UpcService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UpcProduct?> LookupUpcAsync(string upc)
    {
        if (string.IsNullOrWhiteSpace(upc))
            return null;

        try
        {
            // Clean UPC - remove spaces and special characters
            upc = System.Text.RegularExpressions.Regex.Replace(upc, @"[^\d]", "");

            if (upc.Length < 8)
                return new UpcProduct { Found = false, Upc = upc };

            var response = await _httpClient.GetAsync($"{UPC_API_URL}?upc={upc}");
            
            if (!response.IsSuccessStatusCode)
                return new UpcProduct { Found = false, Upc = upc };

            var data = await response.Content.ReadFromJsonAsync<UpcApiResponse>();
            
            if (data?.Items == null || data.Items.Count == 0)
                return new UpcProduct { Found = false, Upc = upc };

            var item = data.Items[0];
            
            return new UpcProduct
            {
                Upc = upc,
                Title = item.Title ?? string.Empty,
                Brand = item.Brand ?? string.Empty,
                Description = item.Description ?? string.Empty,
                ImageUrl = item.Image,
                Price = item.LowestPrice ?? null,
                Retailer = item.Retailers?.FirstOrDefault()?.Name,
                Found = true
            };
        }
        catch
        {
            return new UpcProduct { Found = false, Upc = upc };
        }
    }

    // Internal API response models
    private class UpcApiResponse
    {
        public int Code { get; set; }
        public List<UpcItem>? Items { get; set; }
    }

    private class UpcItem
    {
        public string? Title { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal? LowestPrice { get; set; }
        public List<RetailerInfo>? Retailers { get; set; }
    }

    private class RetailerInfo
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
