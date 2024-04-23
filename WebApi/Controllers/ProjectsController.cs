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
            var project = await _projectService.GetProjectByIdAsync(id);
            return Ok(project);
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
            await _projectService.DeleteProjectAsync(id);
            return Ok($"Project with Id: {id} has been deleted");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _projectService.UpdateProjectAsync(id, project);
            return Ok("Project has been updated");
        }

        [HttpPost("{projectId}/tasks/{taskId}")]
        public async Task<IActionResult> AddTaskToProject(int taskId, int projectId)
        {
            await _projectService.AddTaskToProjectAsync(taskId, projectId);
            return Ok($"Task with ID: {taskId} has been added to Project with ID: {projectId}");
        }

        [HttpDelete("{projectId}/tasks/{taskId}")]
        public async Task<IActionResult> RemoveTaskFromProject(int projectId, int taskId)
        {
            await _projectService.RemoveTaskFromProjectAsync(projectId, taskId);
            return Ok($"Task {taskId} has been removed from Project {projectId}");
        }

        [HttpGet("{projectId}/tasks")]
        public async Task<IActionResult> GetAllTaskFromProject(int projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            return Ok(project.Tasks);
        }

        // FILES

        [HttpPost("{id}/files/upload")]
        public async Task<IActionResult> UploadFile(int id)
        {
            var httpRequest = HttpContext.Request;

            if (!httpRequest.HasFormContentType)
                return BadRequest("Please include a file into your request.");

            var file = httpRequest.Form.Files[0];

            using (var stream = file.OpenReadStream())
                await _fileService.UploadFileProjectAsync(id, stream, file.FileName);

            return Ok("File upload complete!");
        }

        [HttpGet("{id}/files/list")]
        public async Task<IActionResult> GetListOfFilesAsync(int id)
        {
            return Ok(await _fileService.GetListOfFilesFromProjectAsync(id));
        }

        [HttpGet("{id}/files/view/{fileName}")]
        public async Task<IActionResult> DownloadFileAsync(int id, string fileName)
        {
            return File(await _fileService.DownloadFileFromProjectAsync(id, fileName), "image/webp");
            
        }

    } 
}
