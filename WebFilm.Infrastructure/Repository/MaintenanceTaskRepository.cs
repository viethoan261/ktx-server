using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFilm.Core.Enitites.Maintenance;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class MaintenanceTaskRepository : BaseRepository<int, MaintenanceTask>, IMaintenanceTaskRepository
    {
        public MaintenanceTaskRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<MaintenanceTask> GetAllTasks()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    ORDER BY scheduledDate DESC";
                
                var tasks = SqlConnection.Query<MaintenanceTask>(sqlCommand);
                SqlConnection.Close();
                return tasks;
            }
        }
        
        public MaintenanceTask GetTaskById(int id)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    WHERE id = @id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                
                var task = SqlConnection.Query<MaintenanceTask>(sqlCommand, parameters).FirstOrDefault();
                SqlConnection.Close();
                return task;
            }
        }
        
        public int CreateTask(MaintenanceTask task)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    INSERT INTO `maintenance_tasks` (
                        taskType, 
                        title, 
                        description, 
                        location, 
                        scheduledDate, 
                        status, 
                        priority, 
                        assignedTo, 
                        notes, 
                        createdDate, 
                        modifiedDate
                    ) VALUES (
                        @taskType, 
                        @title, 
                        @description, 
                        @location, 
                        @scheduledDate, 
                        @status, 
                        @priority, 
                        @assignedTo, 
                        @notes, 
                        @createdDate, 
                        @modifiedDate
                    );
                    SELECT LAST_INSERT_ID();";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@taskType", task.taskType);
                parameters.Add("@title", task.title);
                parameters.Add("@description", task.description);
                parameters.Add("@location", task.location);
                parameters.Add("@scheduledDate", task.scheduledDate);
                parameters.Add("@status", task.status);
                parameters.Add("@priority", task.priority);
                parameters.Add("@assignedTo", task.assignedTo);
                parameters.Add("@notes", task.notes);
                parameters.Add("@createdDate", task.createdDate ?? DateTime.Now);
                parameters.Add("@modifiedDate", task.modifiedDate ?? DateTime.Now);
                
                var id = SqlConnection.ExecuteScalar<int>(sqlCommand, parameters);
                task.id = id; // Update the entity with the new ID
                
                SqlConnection.Close();
                return id;
            }
        }
        
        public int UpdateTask(int id, MaintenanceTask task)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    UPDATE `maintenance_tasks` 
                    SET 
                        taskType = @taskType, 
                        title = @title, 
                        description = @description, 
                        location = @location, 
                        scheduledDate = @scheduledDate, 
                        status = @status, 
                        priority = @priority, 
                        assignedTo = @assignedTo, 
                        notes = @notes, 
                        modifiedDate = @modifiedDate
                    WHERE id = @id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@taskType", task.taskType);
                parameters.Add("@title", task.title);
                parameters.Add("@description", task.description);
                parameters.Add("@location", task.location);
                parameters.Add("@scheduledDate", task.scheduledDate);
                parameters.Add("@status", task.status);
                parameters.Add("@priority", task.priority);
                parameters.Add("@assignedTo", task.assignedTo);
                parameters.Add("@notes", task.notes);
                parameters.Add("@modifiedDate", DateTime.Now);
                
                var result = SqlConnection.Execute(sqlCommand, parameters);
                SqlConnection.Close();
                return result;
            }
        }
        
        public int DeleteTask(int id)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    DELETE FROM `maintenance_tasks` 
                    WHERE id = @id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                
                var result = SqlConnection.Execute(sqlCommand, parameters);
                SqlConnection.Close();
                return result;
            }
        }
        
        public IEnumerable<MaintenanceTask> GetTasksByType(string taskType)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    WHERE taskType = @taskType
                    ORDER BY scheduledDate DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@taskType", taskType);
                
                var tasks = SqlConnection.Query<MaintenanceTask>(sqlCommand, parameters);
                SqlConnection.Close();
                return tasks;
            }
        }
        
        public IEnumerable<MaintenanceTask> GetTasksByStatus(string status)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    WHERE status = @status
                    ORDER BY scheduledDate DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@status", status);
                
                var tasks = SqlConnection.Query<MaintenanceTask>(sqlCommand, parameters);
                SqlConnection.Close();
                return tasks;
            }
        }
        
        public IEnumerable<MaintenanceTask> GetTasksByDate(DateTime date)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    WHERE DATE(scheduledDate) = DATE(@date)
                    ORDER BY scheduledDate ASC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@date", date.Date);
                
                var tasks = SqlConnection.Query<MaintenanceTask>(sqlCommand, parameters);
                SqlConnection.Close();
                return tasks;
            }
        }
        
        public IEnumerable<MaintenanceTask> GetTasksByLocation(string location)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    WHERE location = @location
                    ORDER BY scheduledDate DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@location", location);
                
                var tasks = SqlConnection.Query<MaintenanceTask>(sqlCommand, parameters);
                SqlConnection.Close();
                return tasks;
            }
        }
        
        public IEnumerable<MaintenanceTask> GetTasksByAssignee(string assignedTo)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    WHERE assignedTo = @assignedTo
                    ORDER BY scheduledDate DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@assignedTo", assignedTo);
                
                var tasks = SqlConnection.Query<MaintenanceTask>(sqlCommand, parameters);
                SqlConnection.Close();
                return tasks;
            }
        }
        
        public int UpdateTaskStatus(int id, string status, DateTime? completedDate)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    UPDATE `maintenance_tasks` 
                    SET status = @status, 
                        modifiedDate = NOW()";
                
                if (completedDate.HasValue)
                {
                    sqlCommand += ", completedDate = @completedDate";
                }
                
                sqlCommand += " WHERE id = @id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@status", status);
                
                if (completedDate.HasValue)
                {
                    parameters.Add("@completedDate", completedDate);
                }
                
                var result = SqlConnection.Execute(sqlCommand, parameters);
                SqlConnection.Close();
                return result;
            }
        }
        
        public IEnumerable<MaintenanceTask> GetTasksByPriority(string priority)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `maintenance_tasks` 
                    WHERE priority = @priority
                    ORDER BY scheduledDate DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@priority", priority);
                
                var tasks = SqlConnection.Query<MaintenanceTask>(sqlCommand, parameters);
                SqlConnection.Close();
                return tasks;
            }
        }
    }
} 