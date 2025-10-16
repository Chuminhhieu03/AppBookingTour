using AppBookingTour.Application.IServices;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Infrastructure.Services
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<AzureBlobStorageService> _logger;

        public AzureBlobStorageService(IConfiguration configuration, ILogger<AzureBlobStorageService> logger)
        {
            var connectionString = configuration["Azure:Storage:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = configuration["Azure:Storage:ContainerName"] ?? "images";
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(Stream file, string fileName = "")
        {
            // Lấy ra folder chứa ảnh trong blob
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            // Tạo container nếu chưa tồn tại
            await containerClient.CreateIfNotExistsAsync();
            // Thiết lập quyền truy cập công khai
            try
            {
                await containerClient.SetAccessPolicyAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            fileName = string.IsNullOrEmpty(fileName) ? Guid.NewGuid().ToString() : fileName;
            // Đặt tên cho file được upload
            var blobClient = containerClient.GetBlobClient(fileName);
            try
            {
                // Upload file lên blob
                await blobClient.UploadAsync(file, new BlobHttpHeaders { ContentType = "image/jpeg" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải file lên blob {FileName}", fileName);
                throw new InvalidOperationException(
                $"Không thể tải file '{fileName}' lên Azure Blob Storage. Vui lòng kiểm tra kết nối hoặc cấu hình container.",
                ex);
            }
            _logger.LogInformation("Tải file thành công lên blob {FileName}", fileName);
            // Trả về URL của file đã upload
            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobName = Path.GetFileName(new Uri(fileUrl).LocalPath);
            var blobClient = containerClient.GetBlobClient(blobName);
            try
            {
                await blobClient.DeleteIfExistsAsync();
                _logger.LogInformation("Xóa file thành công từ blob {FileName}", blobName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa file từ blob {FileUrl}", fileUrl);
                throw;
            }
        }

        public async Task<Stream> DownloadFileAsync(string fileUrl)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobName = Path.GetFileName(new Uri(fileUrl).LocalPath);
            var blobClient = containerClient.GetBlobClient(blobName);
            try
            {
                var downloadInfo = await blobClient.DownloadAsync();
                _logger.LogInformation("Tải file thành công từ blob {FileName}", blobName);
                return downloadInfo.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải file từ blob {FileUrl}", fileUrl);
                throw;
            }
        }
    }
}
