using Microsoft.AspNetCore.Mvc;
using BLL;
using DAL.Entities;
using BLL.Exceptions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetAllProjectAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                return Ok(project);
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _projectService.AddProjectAsync(project);
            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);
                return Ok($"Project with Id: {id} has been deleted");
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _projectService.UpdateProjectAsync(id, project);
                return Ok("Project has been updated");
            }
            catch(ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ProjectTasksNotCompletedException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{projectId}/tasks/{taskId}")]
        public async Task<IActionResult> AddTaskToProject(int taskId, int projectId)
        {
            try
            {
                await _projectService.AddTaskToProjectAsync(taskId, projectId);
                return Ok($"Task with ID: {taskId} has been added to Project with ID: {projectId}");
            }
            catch(ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(TaskNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{projectId}/tasks/{taskId}")]
        public async Task<IActionResult> RemoveTaskFromProject(int projectId, int taskId)
        {
            try
            {
                await _projectService.RemoveTaskFromProjectAsync(projectId, taskId);
                return Ok($"Task {taskId} has been removed from Project {projectId}");
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (TaskNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (TaskNotFoundOnProjectException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("{projectId}/tasks")]
        public async Task<IActionResult> GetAllTaskFromProject(int projectId)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(projectId);
                return Ok(project.Tasks);
            }
            catch(ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("hello")]
        public async Task<IActionResult> HelloFromMe()
        {
            return Ok("Test was ok!");
        }


    } 
}
