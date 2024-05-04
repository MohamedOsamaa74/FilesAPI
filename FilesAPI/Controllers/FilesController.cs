using FilesAPI.Interfaces;
using FilesAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFileRepo _fileRepo;
        public FilesController(IFileService fileService,IFileRepo fileRepo)
        {
            _fileService = fileService;
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
            var attachement = await _fileService.UploadFileAsync(file);
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

        #region ViewFile
        [HttpGet("View-File{folderName}/{fileName}")]
        public async Task<IActionResult> ViewFileAsync(string folderName, string fileName)
        {
            var fileEntity = await _fileRepo.GetByNameAsync(fileName);
            var file = await _fileService.DownloadFileAsync(folderName, fileName);
            if (file == null)
            {
                return NotFound();
            }
            return File(file, fileEntity.Type);
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