using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Authorize] // Assuming authorization is needed, like other controllers
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRoomStatistics()
        {
            try
            {
                var result = await _statisticsService.GetRoomStatistics();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Consider more specific error handling/logging
                return StatusCode(500, new { devMsg = ex.Message, userMsg = "An error occurred while fetching room statistics." });
            }
        }

        [HttpGet("bills")]
        public async Task<IActionResult> GetMonthlyBillStatistics([FromQuery] int? year = null)
        {
            try
            {
                var result = await _statisticsService.GetMonthlyBillStatistics(year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Consider more specific error handling/logging
                return StatusCode(500, new { devMsg = ex.Message, userMsg = "An error occurred while fetching bill statistics." });
            }
        }
    }
} 