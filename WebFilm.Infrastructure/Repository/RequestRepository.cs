using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using WebFilm.Core.Enitites.Request;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class RequestRepository : BaseRepository<int, Request>, IRequestRepository
    {
        public RequestRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Request CreateRequest(Request request)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    INSERT INTO requests (
                        studentId, title, content, type, status, 
                        createdDate, modifiedDate
                    ) VALUES (
                        @v_StudentId, @v_Title, @v_Content, @v_Type, 'PENDING', 
                        NOW(), NOW()
                    );
                    SELECT LAST_INSERT_ID();";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_StudentId", request.studentId);
                parameters.Add("v_Title", request.title);
                parameters.Add("v_Content", request.content);
                parameters.Add("v_Type", request.type);
                
                var requestId = SqlConnection.ExecuteScalar<int>(sqlCommand, parameters);
                
                if (requestId > 0)
                {
                    var createdRequest = SqlConnection.QueryFirstOrDefault<Request>(
                        "SELECT * FROM requests WHERE id = @v_Id", 
                        new { v_Id = requestId }
                    );
                    
                    SqlConnection.Close();
                    return createdRequest;
                }
                
                SqlConnection.Close();
                return null;
            }
        }

        public Request UpdateRequestStatus(int id, string status, string response)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                string sqlCommand;
                DynamicParameters parameters = new DynamicParameters();
                
                parameters.Add("v_Id", id);
                parameters.Add("v_Status", status);
                parameters.Add("v_Response", response);
                
                if (status == "RESOLVED")
                {
                    sqlCommand = @"
                        UPDATE requests 
                        SET status = @v_Status, 
                            response = @v_Response, 
                            modifiedDate = NOW(),
                            resolvedDate = NOW()
                        WHERE id = @v_Id";
                }
                else
                {
                    sqlCommand = @"
                        UPDATE requests 
                        SET status = @v_Status, 
                            response = @v_Response, 
                            modifiedDate = NOW()
                        WHERE id = @v_Id";
                }
                
                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);
                
                if (affectedRows > 0)
                {
                    var updatedRequest = SqlConnection.QueryFirstOrDefault<Request>(
                        "SELECT * FROM requests WHERE id = @v_Id", 
                        new { v_Id = id }
                    );
                    
                    SqlConnection.Close();
                    return updatedRequest;
                }
                
                SqlConnection.Close();
                return null;
            }
        }

        public List<RequestDetailDTO> GetAllRequests()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT r.*, u.fullname as studentName
                    FROM requests r
                    JOIN Users u ON r.studentId = u.id
                    ORDER BY 
                      CASE 
                        WHEN r.status = 'PENDING' THEN 1
                        WHEN r.status = 'PROCESSING' THEN 2
                        WHEN r.status = 'RESOLVED' THEN 3
                      END,
                      r.createdDate DESC";
                
                var requests = SqlConnection.Query<RequestDetailDTO>(sqlCommand).ToList();
                
                SqlConnection.Close();
                return requests;
            }
        }

        public List<RequestDetailDTO> GetRequestsByStudentId(int studentId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT r.*, u.fullname as studentName
                    FROM requests r
                    JOIN Users u ON r.studentId = u.id
                    WHERE r.studentId = @v_StudentId
                    ORDER BY r.createdDate DESC";
                
                var parameters = new DynamicParameters();
                parameters.Add("v_StudentId", studentId);
                
                var requests = SqlConnection.Query<RequestDetailDTO>(sqlCommand, parameters).ToList();
                
                SqlConnection.Close();
                return requests;
            }
        }

        public RequestDetailDTO GetRequestById(int id)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT r.*, u.fullname as studentName
                    FROM requests r
                    JOIN Users u ON r.studentId = u.id
                    WHERE r.id = @v_Id";
                
                var parameters = new DynamicParameters();
                parameters.Add("v_Id", id);
                
                var request = SqlConnection.QueryFirstOrDefault<RequestDetailDTO>(sqlCommand, parameters);
                
                SqlConnection.Close();
                return request;
            }
        }
    }
} 