using DAL.Entities;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace DAL.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectDbContext _dbContext;

        public ProjectRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _dbContext.Projects.ToListAsync();
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await _dbContext.Projects.Include(t => t.Tasks).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task DeleteAsync(Project project)
        {
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddAsync(Project project)
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Project project)
        {
            _dbContext.Entry(project).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
