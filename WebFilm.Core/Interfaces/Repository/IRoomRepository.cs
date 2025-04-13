using WebFilm.Core.Enitites.Room;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IRoomRepository : IBaseRepository<int, Rooms>
    {
        Rooms CreateRoom(Rooms room);
        bool AddStudentsToRoom(int roomId, List<int> studentIds);
        List<RoomResponseDTO> GetAllRoomsWithStudents();
        Rooms UpdateRoom(int id, Rooms room);
        bool RemoveAllStudentsFromRoom(int roomId);
    }
} 