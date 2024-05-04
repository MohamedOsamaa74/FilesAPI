using FilesAPI.Entities;
using FilesAPI.Interfaces;
using FilesAPI.Repositories;

namespace FilesAPI.Services
{
    public class FileService : IFileService
    {
        #region Fields
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IFileRepo _fileRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileService(AppDbContext context, IWebHostEnvironment env, IFileRepo fileRepo, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _fileRepo = fileRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region DeleteFile
        public async Task<bool> DeleteFileAsync(string folderName, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                var attachement = _context.Files.Where(f => f.FilePath == filePath).SingleOrDefault();
                if(attachement != null)
                {
                    _context.Files.Remove(attachement);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
        #endregion

        #region GetFilePath
        public async Task<string> GetFilePathAsync(string folderName, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if (File.Exists(filePath))
            {
                return filePath;
            }
            return null;
        }
        #endregion

        #region DownloadFile
        public async Task<byte[]> DownloadFileAsync(string folderName, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, folderName, fileName);
            if(filePath is null)
            {
                return null;
            }
            return await File.ReadAllBytesAsync(filePath);
        }
        #endregion

        #region UploadFile
        public async Task<Attachement> UploadFileAsync(IFormFile file)
        {
            var folderName = "files";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
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
            var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                      $"{_httpContextAccessor.HttpContext.Request.Host}" +
                      $"/api/files/{folderName}/{fileName}";
            var attachement = new Attachement
            {
                Title = file.FileName,
                FilePath = filePath,
                Url = url,
                Type = file.ContentType
            };
            await _context.Files.AddAsync(attachement);
            await _context.SaveChangesAsync();
            return attachement;
        }
        #endregion
    }
}