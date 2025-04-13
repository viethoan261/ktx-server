using WebFilm.Core.Enitites.Room;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IRoomService : IBaseService<int, Rooms>
    {
        Rooms CreateRoom(RoomDTO roomDTO);
        List<RoomResponseDTO> GetAllRoomsWithStudents();
        Rooms UpdateRoom(int id, RoomDTO roomDTO);
        bool DeleteRoom(int id);
    }
} 