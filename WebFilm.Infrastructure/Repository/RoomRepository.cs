using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using WebFilm.Core.Enitites.Room;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class RoomRepository : BaseRepository<int, Rooms>, IRoomRepository
    {
        public RoomRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Rooms CreateRoom(Rooms room)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO rooms (floorNumber, roomNumber, maxOccupancy, status, currentOccupancy, createdDate, modifiedDate)
                                   VALUES (@v_FloorNumber, @v_RoomNumber, @v_MaxOccupancy, @v_Status, @v_CurrentOccupancy, NOW(), NOW());
                                   SELECT LAST_INSERT_ID();";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_FloorNumber", room.floorNumber);
                parameters.Add("v_RoomNumber", room.roomNumber);
                parameters.Add("v_MaxOccupancy", room.maxOccupancy);
                parameters.Add("v_Status", room.status);
                parameters.Add("v_CurrentOccupancy", room.currentOccupancy);
                
                var roomId = SqlConnection.ExecuteScalar<int>(sqlCommand, parameters);
                
                if (roomId > 0)
                {
                    var insertedRoom = SqlConnection.QueryFirstOrDefault<Rooms>("SELECT * FROM rooms WHERE id = @v_Id", new { v_Id = roomId });
                    SqlConnection.Close();
                    return insertedRoom;
                }
                
                SqlConnection.Close();
                return null;
            }
        }

        public bool AddStudentsToRoom(int roomId, List<int> studentIds)
        {
            if (studentIds == null || !studentIds.Any())
                return true;

            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                SqlConnection.Open();
                using (var transaction = SqlConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var studentId in studentIds)
                        {
                            var sqlCommand = $@"INSERT INTO room_student (roomId, studentId, createdDate, modifiedDate)
                                              VALUES (@v_RoomId, @v_StudentId, NOW(), NOW())";
                            
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("v_RoomId", roomId);
                            parameters.Add("v_StudentId", studentId);
                            
                            SqlConnection.Execute(sqlCommand, parameters, transaction);
                        }
                        
                        // Update current occupancy
                        var updateOccupancySql = "UPDATE rooms SET currentOccupancy = @v_Count WHERE id = @v_RoomId";
                        SqlConnection.Execute(updateOccupancySql, new { v_Count = studentIds.Count, v_RoomId = roomId }, transaction);
                        
                        transaction.Commit();
                        return true;
                    }
                    catch
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

        public List<RoomResponseDTO> GetAllRoomsWithStudents()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                // First, get all rooms
                var rooms = SqlConnection.Query<Rooms>("SELECT * FROM rooms").ToList();
                
                // Create result list
                var result = new List<RoomResponseDTO>();
                
                // Process each room
                foreach (var room in rooms)
                {
                    // Create a DTO for this room
                    var roomDTO = new RoomResponseDTO
                    {
                        id = room.id,
                        floorNumber = room.floorNumber,
                        roomNumber = room.roomNumber,
                        maxOccupancy = room.maxOccupancy,
                        status = room.status,
                        currentOccupancy = room.currentOccupancy,
                        createdDate = room.createdDate,
                        modifiedDate = room.modifiedDate,
                        students = new List<Users>()
                    };
                    
                    // Get all students for this room
                    var sqlQuery = @"
                        SELECT u.* 
                        FROM Users u
                        JOIN room_student rs ON u.id = rs.studentId
                        WHERE rs.roomId = @v_RoomId";
                    
                    var students = SqlConnection.Query<Users>(sqlQuery, new { v_RoomId = room.id }).ToList();
                    
                    // Add students to room
                    roomDTO.students = students;
                    
                    // Add room to result
                    result.Add(roomDTO);
                }
                
                SqlConnection.Close();
                return result;
            }
        }

        public Rooms UpdateRoom(int id, Rooms room)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"UPDATE rooms 
                                   SET floorNumber = @v_FloorNumber, 
                                       roomNumber = @v_RoomNumber, 
                                       maxOccupancy = @v_MaxOccupancy, 
                                       status = @v_Status, 
                                       currentOccupancy = @v_CurrentOccupancy, 
                                       modifiedDate = NOW()
                                   WHERE id = @v_Id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_FloorNumber", room.floorNumber);
                parameters.Add("v_RoomNumber", room.roomNumber);
                parameters.Add("v_MaxOccupancy", room.maxOccupancy);
                parameters.Add("v_Status", room.status);
                parameters.Add("v_CurrentOccupancy", room.currentOccupancy);
                parameters.Add("v_Id", id);
                
                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);
                
                if (affectedRows > 0)
                {
                    var updatedRoom = SqlConnection.QueryFirstOrDefault<Rooms>("SELECT * FROM rooms WHERE id = @v_Id", new { v_Id = id });
                    SqlConnection.Close();
                    return updatedRoom;
                }
                
                SqlConnection.Close();
                return null;
            }
        }

        public bool RemoveAllStudentsFromRoom(int roomId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                SqlConnection.Open();
                using (var transaction = SqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Delete all student assignments for this room
                        var sqlCommand = "DELETE FROM room_student WHERE roomId = @v_RoomId";
                        SqlConnection.Execute(sqlCommand, new { v_RoomId = roomId }, transaction);
                        
                        // Update current occupancy to 0
                        var updateOccupancySql = "UPDATE rooms SET currentOccupancy = 0 WHERE id = @v_RoomId";
                        SqlConnection.Execute(updateOccupancySql, new { v_RoomId = roomId }, transaction);
                        
                        transaction.Commit();
                        return true;
                    }
                    catch
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
    }
} 