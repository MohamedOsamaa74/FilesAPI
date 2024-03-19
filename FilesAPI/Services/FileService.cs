using FilesAPI.Entities;
using FilesAPI.Interfaces;
using FilesAPI.Repositories;

namespace FilesAPI.Services
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IFileRepo _fileRepo;
        public FileService(AppDbContext context, IWebHostEnvironment env, IFileRepo fileRepo)
        {
            _context = context;
            _env = env;
            _fileRepo = fileRepo;
        }
        public async Task<bool> DeleteFileAsync(string folderName, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                var attachement = _context.Files.Where(f => f.Title == fileName).SingleOrDefault();
                if(attachement != null)
                {
                    _context.Files.Remove(attachement);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<string> GetFilePathAsync(string folderName, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if (File.Exists(filePath))
            {
                return filePath;
            }
            return null;
        }
        public async Task<byte[]> DownloadFileAsync(string folderName, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if(filePath is null)
            {
                return null;
            }
            return await File.ReadAllBytesAsync(filePath);
            
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folderName, string fileName)
        {
            var folderPath = Path.Combine(_env.WebRootPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
