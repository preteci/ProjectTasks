using DAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace DAL.Repository
{
    public interface IProjectRepository
    {
        public Task<IEnumerable<Project>> GetAllAsync();
        public Task<Project> GetByIdAsync(int id);
        public Task DeleteAsync(Project project);
        public Task AddAsync(Project project);
        public Task UpdateAsync(Project project);
        public Task SaveChangesAsync();

    }
}
