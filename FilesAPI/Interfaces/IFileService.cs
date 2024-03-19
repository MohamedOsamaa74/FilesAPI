namespace FilesAPI.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, string fileName);
        Task<byte[]> DownloadFileAsync(string folderName, string fileName);
        Task<bool> DeleteFileAsync(string folderName, string fileName);
        Task<string> GetFilePathAsync(string folderName, string fileName);
    }
}
