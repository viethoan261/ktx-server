using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Newtonsoft.Json;
using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;
using BCrypt.Net;

namespace WebFilm.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<int, Users>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #region Method
        public Users GetUserByID(int userID)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT * FROM Users WHERE ID = @v_UserID";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserID", userID);
                var user = SqlConnection.QueryFirstOrDefault<Users>(sqlCommand, parameters);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return user;
            }
        }

        public Users Signup(UserDTO user)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO Users (fullname, username, password, role, createdDate, modifiedDate, phone, email)
                                              VALUES (@v_FullName, @v_UserName, @v_Password, @v_Role, NOW(), NOW(), @v_phone, @v_email);";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserName", user.username);
                parameters.Add("v_Password", user.password);
                parameters.Add("v_FullName", user.fullname);
                parameters.Add("v_Role", user.role);
                parameters.Add("v_phone", user.phone);
                parameters.Add("v_email", user.email);
                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);

                if (affectedRows > 0)
                {
                    var res = SqlConnection.QueryFirstOrDefault<Users>("SELECT * FROM Users WHERE UserName = @v_UserName", parameters);
                    SqlConnection.Close();
                    return res;
                }

                //Trả dữ liệu về client
                SqlConnection.Close();
                return null;
            }
        }

        public Users Login(string username)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $"SELECT * FROM Users WHERE UserName = @v_UserName";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_UserName", username);
                var res = SqlConnection.QueryFirstOrDefault<Users>(sqlCommand, parameters);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return res;
            }
        }

        public bool CheckDuplicateUsername(string username)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCheck = "SELECT * FROM Users WHERE UserName = @v_UserName";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_UserName", username);

                var res = SqlConnection.QueryFirstOrDefault<Users>(sqlCheck, parameters);
                if (res != null)
                {
                    SqlConnection.Close();
                    return true;
                }
                SqlConnection.Close();
                return false;
            }
        }

        public Users getUserByUsername(string username)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT * FROM User WHERE  UserName = @v_UserName";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserName", username);
                var user = SqlConnection.QueryFirstOrDefault<Users>(sqlCommand, parameters);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return user;
            }
        }

        public Users changeProfile(string username, ChangeProfileDTO dto)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"Update `Users` set fullname = @v_fullname, salary = @v_salary, image = @v_image, ModifiedDate = NOW() where username = @v_username;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_fullname", dto.fullName);
                parameters.Add("v_salary", dto.salary);
                parameters.Add("v_image", dto.image);
                parameters.Add("v_username", username);

                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);

                if (affectedRows > 0)
                {
                    var res = SqlConnection.QueryFirstOrDefault<Users>("SELECT * FROM `Users` WHERE username = @v_username", parameters);
                    SqlConnection.Close();
                    return res;
                }
                //Trả dữ liệu về client
                SqlConnection.Close();
                return null;
            }
        }

        public List<Users> getAllStudents(string className)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT * FROM Users WHERE className = @v_className and role = 'STUDENT'";
                var parameters = new DynamicParameters();
                parameters.Add("v_className", className);

                return connection.Query<Users>(sqlCommand, parameters).ToList();
            }
        }

        public Users UpdateUser(int id, UserDTO userDTO)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"UPDATE Users 
                                   SET fullname = @v_FullName, 
                                       username = @v_UserName, 
                                       role = @v_Role,
                                       email = @v_Email,
                                       phone = @v_Phone,
                                       modifiedDate = NOW()
                                   WHERE id = @v_ID";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserName", userDTO.username);
                parameters.Add("v_FullName", userDTO.fullname);
                parameters.Add("v_Role", userDTO.role);
                parameters.Add("v_Email", userDTO.email);
                parameters.Add("v_Phone", userDTO.phone);
                parameters.Add("v_ID", id);

                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);

                if (affectedRows > 0)
                {
                    var res = SqlConnection.QueryFirstOrDefault<Users>("SELECT * FROM Users WHERE id = @v_ID", parameters);
                    SqlConnection.Close();
                    return res;
                }

                SqlConnection.Close();
                return null;
            }
        }

        public List<Users> GetUnassignedStudents()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                // Query to get students who are not assigned to any room
                var sqlCommand = @"
                    SELECT u.* 
                    FROM Users u
                    WHERE u.role = 'STUDENT'
                    AND u.id NOT IN (
                        SELECT rs.studentId 
                        FROM room_student rs
                    )";
                
                var students = SqlConnection.Query<Users>(sqlCommand).ToList();
                
                SqlConnection.Close();
                return students;
            }
        }

        public List<StudentResponseDTO> GetAllStudentsWithRoomInfo()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT u.id, u.username, u.password, u.fullname, u.email, u.phone, r.roomNumber
                    FROM Users u
                    LEFT JOIN room_student rs ON u.id = rs.studentId
                    LEFT JOIN rooms r ON rs.roomId = r.id
                    WHERE u.role = 'STUDENT'
                    ORDER BY u.id";
                
                var students = SqlConnection.Query<StudentResponseDTO>(sqlCommand).ToList();
                
                SqlConnection.Close();
                return students;
            }
        }

        public bool DeleteUserWithRoomAssignments(int userId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                SqlConnection.Open();
                using (var transaction = SqlConnection.BeginTransaction())
                {
                    try
                    {
                        // 1. Delete from room_student table
                        string deleteRoomStudentSql = "DELETE FROM room_student WHERE studentId = @v_StudentId";
                        SqlConnection.Execute(deleteRoomStudentSql, new { v_StudentId = userId }, transaction);
                        
                        // 2. Update room occupancy counts
                        string updateRoomsSql = @"
                            UPDATE rooms r
                            SET r.currentOccupancy = (
                                SELECT COUNT(*) 
                                FROM room_student rs 
                                WHERE rs.roomId = r.id
                            )";
                        SqlConnection.Execute(updateRoomsSql, null, transaction);
                        
                        // 3. Delete the user
                        string deleteUserSql = "DELETE FROM Users WHERE id = @v_UserId";
                        int result = SqlConnection.Execute(deleteUserSql, new { v_UserId = userId }, transaction);
                        
                        transaction.Commit();
                        
                        return result > 0;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        SqlConnection.Close();
                    }
                }
            }
        }

        public Users UpdateStudent(int id, UpdateStudentDTO studentDTO)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"UPDATE Users 
                                   SET fullname = @v_FullName,
                                       email = @v_Email,
                                       phone = @v_Phone,
                                       modifiedDate = NOW()
                                   WHERE id = @v_ID AND role = 'STUDENT'";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_FullName", studentDTO.fullname);
                parameters.Add("v_Email", studentDTO.email);
                parameters.Add("v_Phone", studentDTO.phone);
                parameters.Add("v_ID", id);
                
                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);
                
                if (affectedRows > 0)
                {
                    var updatedStudent = SqlConnection.QueryFirstOrDefault<Users>("SELECT * FROM Users WHERE id = @v_ID", new { v_ID = id });
                    SqlConnection.Close();
                    return updatedStudent;
                }
                
                SqlConnection.Close();
                return null;
            }
        }

        #endregion
    }
}
