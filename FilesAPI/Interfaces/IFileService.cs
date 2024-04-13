using FilesAPI.Entities;

namespace FilesAPI.Interfaces
{
    public interface IFileService
    {
        public Task<Attachement> UploadFileAsync(IFormFile file);
        public Task<byte[]> DownloadFileAsync(string folderName, string fileName);
        public Task<bool> DeleteFileAsync(string folderName, string fileName);
        public Task<string> GetFilePathAsync(string folderName, string fileName);
    }
}
