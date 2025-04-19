using WebFilm.Core.Enitites.Maintenance;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IMaintenanceTaskService : IBaseService<int, MaintenanceTask>
    {
        IEnumerable<MaintenanceTaskResponseDTO> GetAllTasks();
        
        MaintenanceTaskResponseDTO GetTaskById(int id);
        
        MaintenanceTaskResponseDTO CreateTask(MaintenanceTaskDTO taskDTO);
        
        MaintenanceTaskResponseDTO UpdateTask(int id, MaintenanceTaskDTO taskDTO);
        
        int DeleteTask(int id);
        
        IEnumerable<MaintenanceTaskResponseDTO> GetTasksByType(string taskType);
        
        IEnumerable<MaintenanceTaskResponseDTO> GetTasksByStatus(string status);
        
        IEnumerable<MaintenanceTaskResponseDTO> GetTasksByDate(DateTime date);
        
        IEnumerable<MaintenanceTaskResponseDTO> GetTasksByLocation(string location);
        
        IEnumerable<MaintenanceTaskResponseDTO> GetTasksByAssignee(string assignedTo);
        
        IEnumerable<MaintenanceTaskResponseDTO> GetTasksByPriority(string priority);
        
        MaintenanceTaskResponseDTO UpdateTaskStatus(int id, string status);
        
        MaintenanceTaskResponseDTO CompleteTask(int id);
        
        IEnumerable<MaintenanceTaskResponseDTO> SearchTasks(string type, string status, string priority);
    }
} 