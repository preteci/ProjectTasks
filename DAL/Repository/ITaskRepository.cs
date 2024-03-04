using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Task = System.Threading.Tasks.Task;
using DataTask = DAL.Entities.Task;

namespace DAL.Repository
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<DataTask>> GetAllAsync();
        public Task<DataTask> GetByIdAsync(int taskId);
        public Task AddAsync(DataTask Task);
        public Task UpdateAsync(DataTask Task);
        public Task DeleteAsync(DataTask Task);
        public Task SaveChangesAsync();
    }
}
