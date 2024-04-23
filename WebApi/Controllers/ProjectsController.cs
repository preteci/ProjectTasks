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
        private readonly IFileService _fileService;
        public ProjectsController(IProjectService projectService, IFileService fileService)
        {
            _projectService = projectService;
            _fileService = fileService; 
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

        // FILES

        [HttpPost("{id}/files/upload")]
        public async Task<IActionResult> UploadFile(int id)
        {
            var httpRequest = HttpContext.Request;
            if (!httpRequest.HasFormContentType)
                return BadRequest("Please include a file into your request.");
            try
            {
                var file = httpRequest.Form.Files[0];

                using (var stream = file.OpenReadStream())
                {
                    await _fileService.UploadFileProjectAsync(id, stream, file.FileName);
                }

                return Ok("File upload complete!");
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/files/list")]
        public async Task<IActionResult> GetListOfFilesAsync(int id)
        {
            try
            {
                return Ok(await _fileService.GetListOfFilesFromProjectAsync(id));
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/files/view/{fileName}")]
        public async Task<IActionResult> DownloadFileAsync(int id, string fileName)
        {
            try
            {
                return File(await _fileService.DownloadFileFromProjectAsync(id, fileName), "image/webp");
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    } 
}
