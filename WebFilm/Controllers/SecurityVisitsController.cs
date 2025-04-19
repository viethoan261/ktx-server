using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebFilm.Core.Enitites.Security;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityVisitsController : BaseController<int, SecurityVisit>
    {
        private readonly ISecurityVisitService _securityVisitService;

        public SecurityVisitsController(ISecurityVisitService securityVisitService)
            : base(securityVisitService)
        {
            _securityVisitService = securityVisitService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetAllVisits()
        {
            try
            {
                var visits = _securityVisitService.GetAllVisits();
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("active")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetActiveVisits()
        {
            try
            {
                var visits = _securityVisitService.GetActiveVisits();
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("date/{date}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetVisitsByDate(string date)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
                }
                
                var visits = _securityVisitService.GetVisitsByDate(parsedDate);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("student/{studentId}")]
        [Authorize]
        public IActionResult GetVisitsByStudentId(int studentId)
        {
            try
            {
                var visits = _securityVisitService.GetVisitsByStudentId(studentId);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("checkin")]
        [Authorize]
        public IActionResult CheckIn([FromBody] SecurityVisitDTO visitDTO)
        {
            try
            {
                var visit = _securityVisitService.CheckIn(visitDTO);
                return Ok(visit);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/checkout")]
        [Authorize]
        public IActionResult CheckOut(int id)
        {
            try
            {
                var visit = _securityVisitService.CheckOut(id);
                if (visit == null)
                {
                    return NotFound($"Visit with ID {id} not found or already checked out.");
                }
                return Ok(visit);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetVisitById(int id)
        {
            try
            {
                var visit = _securityVisitService.GetVisitById(id);
                if (visit == null)
                {
                    return NotFound($"Visit with ID {id} not found.");
                }
                return Ok(visit);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult DeleteVisit(int id)
        {
            try
            {
                var result = _securityVisitService.DeleteVisit(id);
                if (result == 0)
                {
                    return NotFound($"Visit with ID {id} not found.");
                }
                return Ok(new { message = "Visit deleted successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
} 