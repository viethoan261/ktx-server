using WebFilm.Core.Enitites.Room;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IRoomStudentRepository : IBaseRepository<int, RoomStudent>
    {
        bool AddStudentToRoom(int roomId, int studentId);
    }
} 