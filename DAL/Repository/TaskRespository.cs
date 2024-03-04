using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using DataTask = DAL.Entities.Task;

namespace DAL.Repository
{
    public class TaskRespository : ITaskRepository
    {
        private readonly ProjectDbContext _dbContext;

        public TaskRespository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DataTask>> GetAllAsync()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public async Task<DataTask> GetByIdAsync(int taskId)
        {
            return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task AddAsync(DataTask Task)
        {
            _dbContext.Tasks.Add(Task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DataTask Task)
        {
            _dbContext.Tasks.Remove(Task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DataTask Task)
        {
            _dbContext.Entry(Task).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async  Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
