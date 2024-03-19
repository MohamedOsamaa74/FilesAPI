using FilesAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilesAPI.Repositories
{
    public class FileRepo : IFileRepo
    {
        private readonly AppDbContext _context;
        public FileRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Attachement attachement)
        {
            await _context.Files.AddAsync(attachement);
            await _context.SaveChangesAsync();
        }
        public async Task<Attachement> GetByIdAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }
        public async Task<Attachement> GetByNameAsync(string Name)
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.FilePath.Contains(Name));
        }
        public async Task<IEnumerable<Attachement>> GetAllAsync()
        {
            return await _context.Files.ToListAsync();
        }
        public async Task UpdateAsync(Attachement attachement)
        {
            _context.Files.Update(attachement);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var attachement = await GetByIdAsync(id);
            _context.Files.Remove(attachement);
            await _context.SaveChangesAsync();
        }
    }
}
