using DAL.Repository;
using Task = System.Threading.Tasks.Task;
using DataTask = DAL.Entities.Task;
using BLL.Exceptions;
using DAL.Entities;

namespace BLL
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<IEnumerable<DataTask>> GetAllTaskAsync()
        {
            return await _taskRepository.GetAllAsync();
        }
        public async Task<DataTask> GetTaskByIdAsync(int Id)
        {
            var task = await _taskRepository.GetByIdAsync(Id);
            if (task == null)
            {
                throw new TaskNotFoundException($"Task with id {Id} is not found");
            }
            return task;
        }
        public async Task AddTaskAsync(DataTask Task)
        {
            await _taskRepository.AddAsync(Task);
        }

        public async Task DeleteTaskAsync(int Id)
        {
            var task = await _taskRepository.GetByIdAsync(Id);
            if (task == null)
            {
                throw new TaskNotFoundException($"Task with id {Id} is not found");
            }
            await _taskRepository.DeleteAsync(task);
        }

        public async Task UpdateTaskAsync(int id, DataTask taskBody)
        {
            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                throw new TaskNotFoundException($"Task not found with Id: {id}");
            }
            existingTask.Name = taskBody.Name;
            existingTask.Description = taskBody.Description;
            existingTask.Status = taskBody.Status;
            await _taskRepository.SaveChangesAsync();
        }
    }
}