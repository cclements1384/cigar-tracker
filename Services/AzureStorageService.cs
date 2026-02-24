using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace cigar_tracker.Services;

public class AzureStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly string _containerName = "cigars";

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
    }

    /// <summary>
    /// Uploads an image to Azure Blob Storage
    /// </summary>
    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string userEmail)
    {
        try
        {
            // Create blob name: cigars/{userEmail}/cigar-{timestamp}-{fileName}
            var timestamp = DateTime.UtcNow.Ticks;
            var blobName = $"cigars/{userEmail}/cigar-{timestamp}-{fileName}";

            var blobClient = _containerClient.GetBlobClient(blobName);

            // Upload the file
            await blobClient.UploadAsync(fileStream, overwrite: true);

            // Return the blob URI
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload image to Azure Storage: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Deletes an image from Azure Blob Storage
    /// </summary>
    public async Task DeleteImageAsync(string imageUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            // Extract blob name from URI
            var uri = new Uri(imageUrl);
            var blobName = uri.AbsolutePath.Substring(uri.AbsolutePath.IndexOf("/", 1) + 1);

            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteAsync();
        }
        catch (Exception ex)
        {
            // Log but don't throw - blob might already be deleted
            Console.WriteLine($"Failed to delete image from Azure Storage: {ex.Message}");
        }
    }

    /// <summary>
    /// Ensures the container exists (call on startup)
    /// </summary>
    public async Task EnsureContainerExistsAsync()
    {
        try
        {
            await _containerClient.CreateIfNotExistsAsync(
                Azure.Storage.Blobs.Models.PublicAccessType.None);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to ensure container exists: {ex.Message}", ex);
        }
    }
}
