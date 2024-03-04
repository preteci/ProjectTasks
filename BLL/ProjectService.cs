
using DAL.Repository;
using DAL.Entities;
using BLL.Exceptions;
using System.Runtime.CompilerServices;
using Task = System.Threading.Tasks.Task;

namespace BLL
{
    public class ProjectService : IProjectService
    { 
        public readonly IProjectRepository _projectRepository;
        public readonly ITaskRepository _taskRepository;

        public ProjectService(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;   
        }

        public async Task<IEnumerable<Project>> GetAllProjectAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int Id)
        {
            var project = await _projectRepository.GetByIdAsync(Id);
            if(project == null)
            {
                throw new ProjectNotFoundException($"Project not found with Id: {Id}");
            }
            return project;
        }

        public async Task AddProjectAsync(Project project)
        {
            await _projectRepository.AddAsync(project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if(project == null)
            {
                throw new ProjectNotFoundException($"Project not found with Id: {id}");
            }
            await _projectRepository.DeleteAsync(project);
        }

        public async Task AddTaskToProjectAsync(int  taskId, int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if(project == null)
            {
                throw new ProjectNotFoundException($"Project not found with Id: {projectId}");
            }
            var task = await _taskRepository.GetByIdAsync(taskId);
            if(task == null)
            {
                throw new TaskNotFoundException($"Task with ID: {taskId} was not found");
            }
            project.Tasks.Add(task);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task RemoveTaskFromProjectAsync(int projectId, int taskId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new ProjectNotFoundException($"Project not found with Id: {projectId}");
            }
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new TaskNotFoundException($"Task with ID: {taskId} was not found");
            }
            if (task.ProjectId != projectId)
            {
                throw new TaskNotFoundOnProjectException($"Task with ID: {taskId} is not part of Project with Id: {projectId}");
            }
            project.Tasks.Remove(task);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(int id, Project projectBody)
        {
            var projectExisting = await _projectRepository.GetByIdAsync(id);
            if (projectExisting == null)
            {
                throw new ProjectNotFoundException($"Project not found with Id: {id}");
            }
            projectExisting.Name = projectBody.Name;
            projectExisting.Description = projectBody.Description;
            projectExisting.Code = projectBody.Code;
            await _projectRepository.SaveChangesAsync();
        }

    }
}
