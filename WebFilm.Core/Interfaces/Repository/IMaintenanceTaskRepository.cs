using WebFilm.Core.Enitites.Maintenance;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IMaintenanceTaskRepository : IBaseRepository<int, MaintenanceTask>
    {
        IEnumerable<MaintenanceTask> GetAllTasks();
        
        MaintenanceTask GetTaskById(int id);
        
        int CreateTask(MaintenanceTask task);
        
        int UpdateTask(int id, MaintenanceTask task);
        
        int DeleteTask(int id);
        
        IEnumerable<MaintenanceTask> GetTasksByType(string taskType);
        
        IEnumerable<MaintenanceTask> GetTasksByStatus(string status);
        
        IEnumerable<MaintenanceTask> GetTasksByDate(DateTime date);
        
        IEnumerable<MaintenanceTask> GetTasksByLocation(string location);
        
        IEnumerable<MaintenanceTask> GetTasksByAssignee(string assignedTo);
        
        IEnumerable<MaintenanceTask> GetTasksByPriority(string priority);
        
        int UpdateTaskStatus(int id, string status, DateTime? completedDate);
    }
} 