using System;
using System.Collections.Generic;
using System.Linq;
using WebFilm.Core.Enitites.Maintenance;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class MaintenanceTaskService : BaseService<int, MaintenanceTask>, IMaintenanceTaskService
    {
        private readonly IMaintenanceTaskRepository _maintenanceTaskRepository;
        private readonly IUserRepository _userRepository;

        public MaintenanceTaskService(IMaintenanceTaskRepository maintenanceTaskRepository, IUserRepository userRepository)
            : base(maintenanceTaskRepository)
        {
            _maintenanceTaskRepository = maintenanceTaskRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<MaintenanceTaskResponseDTO> GetAllTasks()
        {
            var tasks = _maintenanceTaskRepository.GetAllTasks();
            return MapToResponseDTOs(tasks);
        }

        public MaintenanceTaskResponseDTO GetTaskById(int id)
        {
            var task = _maintenanceTaskRepository.GetTaskById(id);
            if (task == null)
            {
                return null;
            }
            return MapToResponseDTO(task);
        }

        public MaintenanceTaskResponseDTO CreateTask(MaintenanceTaskDTO taskDTO)
        {
            var task = new MaintenanceTask
            {
                taskType = taskDTO.taskType,
                title = taskDTO.title,
                description = taskDTO.description,
                location = taskDTO.location,
                scheduledDate = taskDTO.scheduledDate,
                status = "SCHEDULED",
                priority = taskDTO.priority,
                assignedTo = taskDTO.assignedTo,
                notes = taskDTO.notes,
                createdDate = DateTime.Now,
                modifiedDate = DateTime.Now
            };

            _maintenanceTaskRepository.CreateTask(task);
            return MapToResponseDTO(task);
        }

        public MaintenanceTaskResponseDTO UpdateTask(int id, MaintenanceTaskDTO taskDTO)
        {
            var existingTask = _maintenanceTaskRepository.GetTaskById(id);
            if (existingTask == null)
            {
                return null;
            }

            existingTask.taskType = taskDTO.taskType;
            existingTask.title = taskDTO.title;
            existingTask.description = taskDTO.description;
            existingTask.location = taskDTO.location;
            existingTask.scheduledDate = taskDTO.scheduledDate;
            existingTask.priority = taskDTO.priority;
            existingTask.assignedTo = taskDTO.assignedTo;
            existingTask.notes = taskDTO.notes;
            existingTask.modifiedDate = DateTime.Now;

            _maintenanceTaskRepository.UpdateTask(id, existingTask);
            return MapToResponseDTO(existingTask);
        }

        public int DeleteTask(int id)
        {
            return _maintenanceTaskRepository.DeleteTask(id);
        }

        public IEnumerable<MaintenanceTaskResponseDTO> GetTasksByType(string taskType)
        {
            var tasks = _maintenanceTaskRepository.GetTasksByType(taskType);
            return MapToResponseDTOs(tasks);
        }

        public IEnumerable<MaintenanceTaskResponseDTO> GetTasksByStatus(string status)
        {
            var tasks = _maintenanceTaskRepository.GetTasksByStatus(status);
            return MapToResponseDTOs(tasks);
        }

        public IEnumerable<MaintenanceTaskResponseDTO> GetTasksByDate(DateTime date)
        {
            var tasks = _maintenanceTaskRepository.GetTasksByDate(date);
            return MapToResponseDTOs(tasks);
        }

        public IEnumerable<MaintenanceTaskResponseDTO> GetTasksByLocation(string location)
        {
            var tasks = _maintenanceTaskRepository.GetTasksByLocation(location);
            return MapToResponseDTOs(tasks);
        }

        public IEnumerable<MaintenanceTaskResponseDTO> GetTasksByAssignee(string assignedTo)
        {
            var tasks = _maintenanceTaskRepository.GetTasksByAssignee(assignedTo);
            return MapToResponseDTOs(tasks);
        }

        public MaintenanceTaskResponseDTO UpdateTaskStatus(int id, string status)
        {
            DateTime? completedDate = null;
            
            if (status == "COMPLETED")
            {
                completedDate = DateTime.Now;
            }
            
            var result = _maintenanceTaskRepository.UpdateTaskStatus(id, status, completedDate);
            
            if (result == 0)
            {
                return null; // Task not found
            }
            
            var task = _maintenanceTaskRepository.GetTaskById(id);
            return MapToResponseDTO(task);
        }

        public MaintenanceTaskResponseDTO CompleteTask(int id)
        {
            return UpdateTaskStatus(id, "COMPLETED");
        }

        public IEnumerable<MaintenanceTaskResponseDTO> GetTasksByPriority(string priority)
        {
            var tasks = _maintenanceTaskRepository.GetTasksByPriority(priority);
            return MapToResponseDTOs(tasks);
        }

        public IEnumerable<MaintenanceTaskResponseDTO> SearchTasks(string type, string status, string priority)
        {
            // Get all tasks first
            var tasks = _maintenanceTaskRepository.GetAllTasks();
            
            // Apply filters if parameters are provided
            if (type != "-1")
            {
                tasks = tasks.Where(t => t.taskType == type);
            }
            
            if (status != "-1")
            {
                tasks = tasks.Where(t => t.status == status);
            }
            
            if (priority != "-1")
            {
                tasks = tasks.Where(t => t.priority == priority);
            }
            
            return MapToResponseDTOs(tasks);
        }

        private IEnumerable<MaintenanceTaskResponseDTO> MapToResponseDTOs(IEnumerable<MaintenanceTask> tasks)
        {
            var responseDTOs = new List<MaintenanceTaskResponseDTO>();
            
            foreach (var task in tasks)
            {
                responseDTOs.Add(MapToResponseDTO(task));
            }
            
            return responseDTOs;
        }

        private MaintenanceTaskResponseDTO MapToResponseDTO(MaintenanceTask task)
        {
            string assignedToName = task.assignedTo;
            
            return new MaintenanceTaskResponseDTO
            {
                id = task.id,
                taskType = task.taskType,
                title = task.title,
                description = task.description,
                location = task.location,
                scheduledDate = task.scheduledDate,
                completedDate = task.completedDate,
                status = task.status,
                priority = task.priority,
                assignedTo = task.assignedTo,
                assignedToName = assignedToName,
                notes = task.notes,
                createdDate = task.createdDate,
                modifiedDate = task.modifiedDate
            };
        }
    }
} 