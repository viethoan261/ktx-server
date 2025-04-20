using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Controllers;
using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.Staff;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Services;
using WebFilm.Core.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<int, Users>
    {
        #region Field
        IUserService _userService;
        IUserContext _userContext;

        #endregion

        #region Contructor
        public UsersController(IUserService userService, IUserContext userContext) : base(userService)
        {
            _userService = userService;
            _userContext = userContext;
        }
        #endregion

        #region Method

        [HttpPost("signup")]
        [AllowAnonymous]
        public IActionResult Signup(UserDTO user)
        {
            try
            {
                var res = _userService.Signup(user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                var res = _userService.Login(dto.username, dto.password);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{username}/profile")]
        public IActionResult changeProfile(string username, ChangeProfileDTO dto)
        {
            try
            {
                var res = _userService.changeProfile(username, dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{username}/profile")]
        public IActionResult getProfile(string username)
        {
            try
            {
                var res = _userService.getProfile(username);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("")]
        public IActionResult getAll()
        {
            try
            {
                var res = _userService.getAllStaff();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteUser(int id)
        {
            try
            {
                var res = _userService.deleteUser(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserDTO userDTO)
        {
            try
            {
                var res = _userService.UpdateUser(id, userDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("students/unassigned")]
        public IActionResult GetUnassignedStudents()
        {
            try
            {
                var res = _userService.GetUnassignedStudents();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("students")]
        public IActionResult GetAllStudents()
        {
            try
            {
                var res = _userService.GetAllStudentsWithRoomInfo();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("students/{studentId}/assign-room")]
        public IActionResult AssignRoomToStudent(int studentId, [FromBody] int roomId)
        {
            try
            {
                var result = _userService.AssignRoomToStudent(studentId, roomId);
                return Ok(new { success = result, message = "Gán phòng cho sinh viên thành công" });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("students/{id}")]
        public IActionResult UpdateStudent(int id, UpdateStudentDTO studentDTO)
        {
            try
            {
                var res = _userService.UpdateStudent(id, studentDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        #endregion
    }
}
