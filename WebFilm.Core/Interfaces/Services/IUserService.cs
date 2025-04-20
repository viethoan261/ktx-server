using Microsoft.AspNetCore.Http;
using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.Staff;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IUserService : IBaseService<int, Users>
    {
        Users GetUserByID(int userID);

        Users Signup(UserDTO user);

        Dictionary<string, object> Login(string username, string password);

        Users changeProfile(string username, ChangeProfileDTO dto);

        Users getProfile(string username);

        bool deleteUser(int id);

        List<Users> getAllStaff();

        Users UpdateUser(int id, UserDTO userDTO);

        List<Users> GetUnassignedStudents();

        List<StudentResponseDTO> GetAllStudentsWithRoomInfo();

        Users UpdateStudent(int id, UpdateStudentDTO studentDTO);
        
        bool AssignRoomToStudent(int studentId, int roomId);

        /*List<StudentResponse> getAllStudents(int semesterId, String className);*/
    }
}
