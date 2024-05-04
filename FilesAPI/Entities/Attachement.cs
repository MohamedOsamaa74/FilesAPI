namespace FilesAPI.Entities
{
    public class Attachement
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string FilePath { get; set; }
        public required string Url { get; set; }
        public required string Type { get; set; }
        public DateTime Createion { get; set; } = DateTime.Now.ToLocalTime();
    }
}
