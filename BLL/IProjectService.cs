using DAL.Entities;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BLL
{
    public interface IProjectService
    {
        public Task<IEnumerable<Project>> GetAllProjectAsync();
        public Task<Project> GetProjectByIdAsync(int Id);
        public Task AddProjectAsync(Project project);
        public Task DeleteProjectAsync(int id);
        public Task AddTaskToProjectAsync(int taskId, int projectId);
        public Task RemoveTaskFromProjectAsync(int projectId, int taskId);
        public Task UpdateProjectAsync(int id, Project projectBody);
    }
}
