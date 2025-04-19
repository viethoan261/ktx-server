using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFilm.Core.Enitites.Security;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class SecurityVisitRepository : BaseRepository<int, SecurityVisit>, ISecurityVisitRepository
    {
        public SecurityVisitRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<SecurityVisit> GetAllVisits()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `security_visit` 
                    ORDER BY entryTime DESC";
                
                var visits = SqlConnection.Query<SecurityVisit>(sqlCommand);
                SqlConnection.Close();
                return visits;
            }
        }

        public IEnumerable<SecurityVisit> GetActiveVisits()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `security_visit` 
                    WHERE status = 'CHECKED_IN'
                    ORDER BY entryTime DESC";
                
                var visits = SqlConnection.Query<SecurityVisit>(sqlCommand);
                SqlConnection.Close();
                return visits;
            }
        }

        public IEnumerable<SecurityVisit> GetVisitsByDate(DateTime date)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `security_visit` 
                    WHERE DATE(entryTime) = DATE(@date)
                    ORDER BY entryTime DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@date", date.Date);
                
                var visits = SqlConnection.Query<SecurityVisit>(sqlCommand, parameters);
                SqlConnection.Close();
                return visits;
            }
        }

        public IEnumerable<SecurityVisit> GetVisitsByStudentId(int studentId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `security_visit` 
                    WHERE studentId = @studentId
                    ORDER BY entryTime DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@studentId", studentId);
                
                var visits = SqlConnection.Query<SecurityVisit>(sqlCommand, parameters);
                SqlConnection.Close();
                return visits;
            }
        }

        public int CheckOut(int id, DateTime exitTime)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    UPDATE `security_visit` 
                    SET exitTime = @exitTime, 
                        status = 'CHECKED_OUT', 
                        modifiedDate = NOW() 
                    WHERE id = @id AND status = 'CHECKED_IN'";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@exitTime", exitTime);
                
                var result = SqlConnection.Execute(sqlCommand, parameters);
                SqlConnection.Close();
                return result;
            }
        }

        public int CreateVisit(SecurityVisit visit)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    INSERT INTO `security_visit` (
                        visitorName, 
                        phoneNumber, 
                        studentId, 
                        entryTime, 
                        status, 
                        purpose, 
                        notes, 
                        createdDate, 
                        modifiedDate
                    ) VALUES (
                        @visitorName, 
                        @phoneNumber, 
                        @studentId, 
                        @entryTime, 
                        @status, 
                        @purpose, 
                        @notes, 
                        @createdDate, 
                        @modifiedDate
                    );
                    SELECT LAST_INSERT_ID();";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@visitorName", visit.visitorName);
                parameters.Add("@phoneNumber", visit.phoneNumber);
                parameters.Add("@studentId", visit.studentId);
                parameters.Add("@entryTime", visit.entryTime);
                parameters.Add("@status", visit.status);
                parameters.Add("@purpose", visit.purpose);
                parameters.Add("@notes", visit.notes);
                parameters.Add("@createdDate", visit.createdDate ?? DateTime.Now);
                parameters.Add("@modifiedDate", visit.modifiedDate ?? DateTime.Now);
                
                // Execute the query and get the new ID
                var id = SqlConnection.ExecuteScalar<int>(sqlCommand, parameters);
                visit.id = id; // Update the entity with the new ID
                
                SqlConnection.Close();
                return id;
            }
        }

        public SecurityVisit GetVisitById(int id)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `security_visit` 
                    WHERE id = @id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                
                var visit = SqlConnection.Query<SecurityVisit>(sqlCommand, parameters).FirstOrDefault();
                SqlConnection.Close();
                return visit;
            }
        }

        public int DeleteVisit(int id)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    DELETE FROM `security_visit` 
                    WHERE id = @id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                
                var result = SqlConnection.Execute(sqlCommand, parameters);
                SqlConnection.Close();
                return result;
            }
        }
    }
} 