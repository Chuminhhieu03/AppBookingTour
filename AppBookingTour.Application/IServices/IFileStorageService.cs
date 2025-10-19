namespace AppBookingTour.Application.IServices
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream file, string folderName = "");
        Task<bool> DeleteFileAsync(string fileUrl);
        Task<Stream> DownloadFileAsync(string fileUrl);
    }
}
