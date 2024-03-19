using FilesAPI.Entities;

namespace FilesAPI.Repositories
{
    public interface IFileRepo
    {
        public Task AddAsync(Attachement attachement);
        public Task<Attachement> GetByIdAsync(int id);
        public Task<Attachement> GetByNameAsync(string Name);
        public Task<IEnumerable<Attachement>> GetAllAsync();
        public Task UpdateAsync(Attachement attachement);
        public Task DeleteAsync(int Id);
    }
}
