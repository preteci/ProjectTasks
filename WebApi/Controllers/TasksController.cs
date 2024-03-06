using Microsoft.AspNetCore.Mvc;
using BLL;
using BLL.Exceptions;
using Task = System.Threading.Tasks.Task;
using DataTask = DAL.Entities.Task;
using DAL.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
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
    }
}
