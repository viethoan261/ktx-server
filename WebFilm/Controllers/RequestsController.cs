using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Core.Enitites.Request;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : BaseController<int, Request>
    {
        #region Field
        private readonly IRequestService _requestService;
        private readonly IUserContext _userContext;
        #endregion

        #region Constructor
        public RequestsController(IRequestService requestService, IUserContext userContext) : base(requestService)
        {
            _requestService = requestService;
            _userContext = userContext;
        }
        #endregion

        #region Methods
        [HttpPost]
        public IActionResult CreateRequest(RequestDTO requestDTO)
        {
            try
            {
                // Set studentId to current user if it's a student
                if (_userContext.Role == "STUDENT")
                {
                    requestDTO.studentId = _userContext.UserID;
                }
                
                var res = _requestService.CreateRequest(requestDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/respond")]
        public IActionResult RespondToRequest(int id, RequestResponseDTO responseDTO)
        {
            try
            {
                var res = _requestService.RespondToRequest(id, responseDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAllRequests()
        {
            try
            {
                var res = _requestService.GetAllRequests();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("students/{studentId}")]
        public IActionResult GetStudentRequests(int studentId)
        {
            try
            {
                var res = _requestService.GetStudentRequests(studentId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [HttpGet("my-requests")]
        public IActionResult GetMyRequests()
        {
            try
            {
                var res = _requestService.GetStudentRequests(_userContext.UserID);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRequestById(int id)
        {
            try
            {
                var res = _requestService.GetRequestById(id);
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