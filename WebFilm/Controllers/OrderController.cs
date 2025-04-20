using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<List<Order>>> CreateOrders([FromBody] OrderCreateRequest request)
        {
            try
            {
                var result = await _orderService.CreateOrders(request.orders);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrders()
        {
            try
            {
                var result = await _orderService.GetAllOrders();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(int id)
        {
            try
            {
                var result = await _orderService.GetOrderById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrdersByStudentId(int studentId)
        {
            try
            {
                var result = await _orderService.GetOrdersByStudentId(studentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<bool>> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateDTO statusUpdate)
        {
            try
            {
                var result = await _orderService.UpdateOrderStatus(id, statusUpdate.status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/markAsPaid")]
        public async Task<ActionResult<bool>> MarkOrderAsPaid(int id)
        {
            try
            {
                var result = await _orderService.MarkOrderAsPaid(id);
                return Ok(new { 
                    success = result,
                    message = "Payment successful. Order has been marked as paid."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
} 