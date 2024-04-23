using Microsoft.AspNetCore.Mvc;
using BLL;
using BLL.Exceptions;
using Task = System.Threading.Tasks.Task;
using DataTask = DAL.Entities.Task;
using DAL.Entities;
using System.Security.Cryptography.X509Certificates;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IFileService _fileService;

        public TasksController(ITaskService taskService, IFileService fileService)
        {
            _taskService = taskService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTaskAsync();
            return Ok(tasks);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTaskById(int Id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(Id);
                return Ok(task);
            }
            catch (TaskNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] DataTask Task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskService.AddTaskAsync(Task);
            return Ok("Task has been added");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTask(int Id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(Id);
                return Ok($"Task with Id: {Id} has been deleted");
            }
            catch (TaskNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] DataTask Task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _taskService.UpdateTaskAsync(id, Task);
                return Ok("Task has been updated");
            }
            catch (TaskNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/files/upload")]
        public async Task<IActionResult> UploadFile(int id)
        {
            var httpRequest = HttpContext.Request;

            if (!httpRequest.HasFormContentType)
            {
                return BadRequest("Please include a file into your request.");
            }

            try
            {
                var file = httpRequest.Form.Files[0];

                using (var stream = file.OpenReadStream())
                {
                    await _fileService.UploadFileToTaskAsync(id, stream, file.FileName);
                }
                return Ok("File upload complete!");
            }
            catch (TaskNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/files/list")]
        public async Task<IActionResult> GetListOfFilesAsync(int id)
        {
            try
            {
                return Ok(await _fileService.GetListOfFilesFromTaskAsync(id));
            }
            catch(TaskNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/files/view/{fileName}")]
        public async Task<IActionResult> DownloadFileAsync(int id, string fileName)
        {
            try
            {
                return File(await _fileService.DownloadFileFromTaskAsync(id, fileName), "image/webp");
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
