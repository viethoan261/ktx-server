using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using WebFilm.Core.Enitites.Room;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class RoomStudentRepository : BaseRepository<int, RoomStudent>, IRoomStudentRepository
    {
        public RoomStudentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public bool AddStudentToRoom(int roomId, int studentId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO room_student (roomId, studentId, createdDate, modifiedDate)
                                   VALUES (@v_RoomId, @v_StudentId, NOW(), NOW())";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_RoomId", roomId);
                parameters.Add("v_StudentId", studentId);
                
                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);
                
                SqlConnection.Close();
                return affectedRows > 0;
            }
        }
    }
} 