using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebFilm.Core.Enitites.Maintenance;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceTasksController : BaseController<int, MaintenanceTask>
    {
        private readonly IMaintenanceTaskService _maintenanceTaskService;

        public MaintenanceTasksController(IMaintenanceTaskService maintenanceTaskService)
            : base(maintenanceTaskService)
        {
            _maintenanceTaskService = maintenanceTaskService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetAllTasks()
        {
            try
            {
                var tasks = _maintenanceTaskService.GetAllTasks();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetTaskById(int id)
        {
            try
            {
                var task = _maintenanceTaskService.GetTaskById(id);
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public IActionResult CreateTask([FromBody] MaintenanceTaskDTO taskDTO)
        {
            try
            {
                var task = _maintenanceTaskService.CreateTask(taskDTO);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult UpdateTask(int id, [FromBody] MaintenanceTaskDTO taskDTO)
        {
            try
            {
                var task = _maintenanceTaskService.UpdateTask(id, taskDTO);
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                var result = _maintenanceTaskService.DeleteTask(id);
                if (result == 0)
                {
                    return NotFound($"Task with ID {id} not found.");
                }
                return Ok(new { message = "Task deleted successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("type/{taskType}")]
        [Authorize]
        public IActionResult GetTasksByType(string taskType)
        {
            try
            {
                var tasks = _maintenanceTaskService.GetTasksByType(taskType);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("status/{status}")]
        [Authorize]
        public IActionResult GetTasksByStatus(string status)
        {
            try
            {
                var tasks = _maintenanceTaskService.GetTasksByStatus(status);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("date/{date}")]
        [Authorize]
        public IActionResult GetTasksByDate(string date)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
                }
                
                var tasks = _maintenanceTaskService.GetTasksByDate(parsedDate);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("location/{location}")]
        [Authorize]
        public IActionResult GetTasksByLocation(string location)
        {
            try
            {
                var tasks = _maintenanceTaskService.GetTasksByLocation(location);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("assignee/{assignedTo}")]
        [Authorize]
        public IActionResult GetTasksByAssignee(string assignedTo)
        {
            try
            {
                var tasks = _maintenanceTaskService.GetTasksByAssignee(assignedTo);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult UpdateTaskStatus(int id, [FromBody] string status)
        {
            try
            {
                var validStatuses = new[] { "SCHEDULED", "IN_PROGRESS", "COMPLETED", "CANCELLED" };
                
                if (!Array.Exists(validStatuses, s => s == status))
                {
                    return BadRequest($"Invalid status. Valid values are: {string.Join(", ", validStatuses)}");
                }
                
                var task = _maintenanceTaskService.UpdateTaskStatus(id, status);
                
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }
                
                return Ok(task);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult CompleteTask(int id)
        {
            try
            {
                var task = _maintenanceTaskService.CompleteTask(id);
                
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }
                
                return Ok(task);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("priority/{priority}")]
        [Authorize]
        public IActionResult GetTasksByPriority(string priority)
        {
            try
            {
                var validPriorities = new[] { "LOW", "MEDIUM", "HIGH", "URGENT" };
                
                if (!Array.Exists(validPriorities, p => p == priority.ToUpper()))
                {
                    return BadRequest($"Invalid priority. Valid values are: {string.Join(", ", validPriorities)}");
                }
                
                var tasks = _maintenanceTaskService.GetTasksByPriority(priority.ToUpper());
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("search")]
        [Authorize]
        public IActionResult SearchTasks([FromQuery] string type = null, [FromQuery] string status = null, [FromQuery] string priority = null)
        {
            try
            {
                var tasks = _maintenanceTaskService.SearchTasks(type, status, priority);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
} 