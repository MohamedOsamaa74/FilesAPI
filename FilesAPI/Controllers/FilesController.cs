using FilesAPI.Entities;
using FilesAPI.Interfaces;
using FilesAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;
        private readonly IFileRepo _fileRepo;
        public FilesController(IFileService fileService, AppDbContext context, IFileRepo fileRepo)
        {
            _fileService = fileService;
            _context = context;
            _fileRepo = fileRepo;
        }

        #region UploadFile
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required");
            }
            var folderName = "files";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = await _fileService.UploadFileAsync(file, folderName, fileName);
            var url = $"{Request.Scheme}://{Request.Host}/api/files/{folderName}/{fileName}";
            //var url = Url.Content($"~/api/files/{folderName}/{fileName}");
            var attachement = new Attachement
            {
                Title = file.FileName,
                FilePath = filePath,
                Url = url,
                Type = file.ContentType
            };
            await _context.Files.AddAsync(attachement);
            await _context.SaveChangesAsync();
            return Ok(attachement);
        }
        #endregion

        #region downloadFile
        [HttpGet("{folderName}/{fileName}")]
        public async Task<IActionResult> DownloadFile(string folderName, string fileName)
        {
            var fileEntity = await _fileRepo.GetByNameAsync(fileName);
            var file = await _fileService.DownloadFileAsync(folderName, fileName);
            if (file == null)
            {
                return NotFound();
            }
            return File(file, fileEntity.Type, fileEntity.Title);
        }
        #endregion

        #region DeleteFile
        [HttpDelete("{folderName}/{fileName}")]
        public async Task<IActionResult> DeleteFile(string folderName, string fileName)
        {
            var result = await _fileService.DeleteFileAsync(folderName, fileName);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

    }
}