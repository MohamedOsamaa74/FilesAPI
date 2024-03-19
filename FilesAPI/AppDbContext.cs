using Microsoft.EntityFrameworkCore;
using FilesAPI.Entities;
namespace FilesAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Attachement> Files { get; set; }
    }
}
