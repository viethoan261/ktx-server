using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Core.Enitites.Room;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : BaseController<int, Rooms>
    {
        #region Field
        private readonly IRoomService _roomService;
        private readonly IUserContext _userContext;
        #endregion

        #region Constructor
        public RoomsController(IRoomService roomService, IUserContext userContext) : base(roomService)
        {
            _roomService = roomService;
            _userContext = userContext;
        }
        #endregion

        #region Methods
        [HttpPost]
        public IActionResult CreateRoom(RoomDTO roomDTO)
        {
            try
            {
                var res = _roomService.CreateRoom(roomDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAllRooms()
        {
            try
            {
                var res = _roomService.GetAllRoomsWithStudents();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, RoomDTO roomDTO)
        {
            try
            {
                var res = _roomService.UpdateRoom(id, roomDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            try
            {
                var res = _roomService.DeleteRoom(id);
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