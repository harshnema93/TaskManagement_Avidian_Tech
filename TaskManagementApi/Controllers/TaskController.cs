using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(string? searchTerm, string? category, string? priority)
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync(searchTerm, category, priority);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tasks.");
                return StatusCode(500, "Internal Server Error");
            }
            
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return Ok(task);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting task by id: {TaskId}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskManagementApi.Models.Task task)
        {
            if (!ModelState.IsValid)
             {
                return BadRequest(ModelState);
              }
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskManagementApi.Models.Task task)
        {
             if (!ModelState.IsValid)
             {
                    return BadRequest(ModelState);
              }
             try
             {
                var updatedTask = await _taskService.UpdateTaskAsync(id, task);
                if (updatedTask == null)
                {
                    return NotFound();
                }
                return Ok(updatedTask);

             }
             catch (Exception ex)
             {
                _logger.LogError(ex, "Error updating task with id: {TaskId}", id);
                return StatusCode(500, "Internal Server Error");
             }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var deleted = await _taskService.DeleteTaskAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with id: {TaskId}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}