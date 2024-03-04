using DAL.Entities;
using Task = System.Threading.Tasks.Task;
using DataTask = DAL.Entities.Task;
namespace BLL
{
    public interface ITaskService
    {
        public Task<IEnumerable<DataTask>> GetAllTaskAsync();
        public Task<DataTask> GetTaskByIdAsync(int Id);
        public Task AddTaskAsync(DataTask Task);
        public Task DeleteTaskAsync(int id);
        public Task UpdateTaskAsync(int id, DataTask taskBody);
    }
}
