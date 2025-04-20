using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OpenCvSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class UserService : BaseService<int, Users>, IUserService
    {
        IUserRepository _userRepository;
        IUserContext _userContext;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, 
            IConfiguration configuration,
            IUserContext userContext) : base(userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userContext = userContext;

        }

        #region Method
        public Users GetUserByID(int userID)
        {
            var user = _userRepository.GetUserByID(userID);
            return user;
        }

        public Users Signup(UserDTO user)
        {
            var isDuplicateUsername = _userRepository.CheckDuplicateUsername(user.username);
            if (isDuplicateUsername)
            {
                throw new ServiceException(Resources.Resource.Error_Duplicate_UserName);
            }

            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            var res = _userRepository.Signup(user);

            return res;
        }

        public Dictionary<string, object> Login(string username, string password)
        {
            var userDto = _userRepository.Login(username);
            if (userDto != null)
            {
              
              var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, userDto.password);
                if (isPasswordCorrect)
                {
                    var token = GenarateToken(userDto);
                    return new Dictionary<string, object>()
                    {
                        { "token", token }
                    };
                }
            }
            throw new ServiceException("Thông tin tài khoản hoặc mật khẩu không chính xác");
        }

        private string GenarateToken(Users user)
        {
            // Authenticate user credentials and get the user's claims
            var claims = new List<Claim>
            {
                new Claim("id", user.id.ToString()),
                new Claim("username", user.username),
                new Claim("role", user.role),
                new Claim("fullname", user.fullname ?? ""),
                // Add any other user claims as needed
            };

            // Generate a symmetric security key using your secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // Create a signing credentials object using the key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set token expiration time
            var expires = DateTime.UtcNow.AddDays(30);

            // Create a JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            // Serialize the token to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return "Bearer " + tokenString;
        }

        public Users changeProfile(string username, ChangeProfileDTO dto)
        {
            var staff = _userRepository.changeProfile(username, dto);
            if (staff != null)
            {
                return staff;
            }
            throw new ServiceException("User không khả dụng");
        }

        public bool deleteUser(int id)
        {
            try
            {
                // Use the repository method to delete the user and their room assignments
                bool success = _userRepository.DeleteUserWithRoomAssignments(id);
                
                if (!success)
                {
                    throw new ServiceException("Không thể xóa người dùng");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Lỗi khi xóa người dùng: {ex.Message}");
            }
        }

        public List<Users> getAllStaff()
        {
            string role = _userContext.Role;
            if ("STAFF".Equals(role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }
            return _userRepository.GetAll().ToList();
        }

        public Users getProfile(string username)
        {
            var profile = _userRepository.GetAll().Where(p => p.username.Equals(username)).FirstOrDefault();

            return profile;
        }

        public Users UpdateUser(int id, UserDTO userDTO)
        {
            // Check if the user exists
            var existingUser = _userRepository.GetUserByID(id);
            if (existingUser == null)
            {
                throw new ServiceException("User không tồn tại");
            }

            // Check if the username is being changed and if it's already taken
            if (!existingUser.username.Equals(userDTO.username))
            {
                var isDuplicateUsername = _userRepository.CheckDuplicateUsername(userDTO.username);
                if (isDuplicateUsername)
                {
                    throw new ServiceException(Resources.Resource.Error_Duplicate_UserName);
                }
            }

            // Update the user
            var updatedUser = _userRepository.UpdateUser(id, userDTO);
            if (updatedUser == null)
            {
                throw new ServiceException("Cập nhật thông tin người dùng thất bại");
            }

            return updatedUser;
        }

        public List<Users> GetUnassignedStudents()
        {  
            // Get students who are not assigned to any room
            return _userRepository.GetUnassignedStudents();
        }

        public List<StudentResponseDTO> GetAllStudentsWithRoomInfo()
        {    
            // Get all students with their room information
            return _userRepository.GetAllStudentsWithRoomInfo();
        }

        public Users UpdateStudent(int id, UpdateStudentDTO studentDTO)
        {
            try
            {
                var existingUser = _userRepository.GetUserByID(id);
                if (existingUser == null)
                {
                    throw new ServiceException("Sinh viên không tồn tại");
                }

                // Check if user is a student
                if (!"STUDENT".Equals(existingUser.role))
                {
                    throw new ServiceException("Người dùng này không phải là sinh viên");
                }

                // Validate input
                if (string.IsNullOrEmpty(studentDTO.fullname))
                {
                    throw new ServiceException("Họ tên không được để trống");
                }

                // Update student
                var updatedStudent = _userRepository.UpdateStudent(id, studentDTO);
                if (updatedStudent == null)
                {
                    throw new ServiceException("Cập nhật thông tin sinh viên thất bại");
                }

                return updatedStudent;
            }
            catch (Exception ex)
            {
                if (ex is ServiceException)
                {
                    throw;
                }
                throw new ServiceException($"Lỗi khi cập nhật sinh viên: {ex.Message}");
            }
        }
        
        public bool AssignRoomToStudent(int studentId, int roomId)
        {
            try
            {
                // Check if student exists
                var student = _userRepository.GetUserByID(studentId);
                if (student == null)
                {
                    throw new ServiceException("Sinh viên không tồn tại");
                }

                // Check if the user is a student
                if (!"STUDENT".Equals(student.role))
                {
                    throw new ServiceException("Người dùng này không phải là sinh viên");
                }

                // Perform the room assignment
                var result = _userRepository.AssignRoomToStudent(studentId, roomId);
                if (!result)
                {
                    throw new ServiceException("Gán phòng cho sinh viên thất bại. Vui lòng kiểm tra xem phòng đã đầy chưa hoặc phòng không tồn tại.");
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex is ServiceException)
                {
                    throw;
                }
                throw new ServiceException($"Lỗi khi gán phòng cho sinh viên: {ex.Message}");
            }
        }
        #endregion
    }
}
