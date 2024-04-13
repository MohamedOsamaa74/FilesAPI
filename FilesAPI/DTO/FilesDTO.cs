namespace FilesAPI.DTO
{
    public class FilesDTO
    {
        public required string FolderName { get; set; }
        public required IFormFile file { get; set; }
    }
}
