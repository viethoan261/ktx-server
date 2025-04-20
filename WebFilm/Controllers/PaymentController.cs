using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Payment;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVNPayService _vnpayService;
        private readonly IUserContext _userContext;
        private readonly IOrderService _orderService;

        public PaymentController(IVNPayService vnpayService, IUserContext userContext, IOrderService orderService)
        {
            _vnpayService = vnpayService;
            _userContext = userContext;
            _orderService = orderService;
        }

        [HttpPost("create-payment")]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] VNPayPaymentRequestDTO paymentInfo)
        {
            try
            {
                // Generate a unique order ID
                paymentInfo.OrderId = paymentInfo.OrderId;
                
                // Get user ID from UserContext
                int userId = _userContext.UserID;
                
                // Save transaction with pending status
                await _vnpayService.SaveTransaction(paymentInfo, userId);
                
                // Create payment URL
                var paymentUrl = _vnpayService.CreatePaymentUrl(HttpContext, paymentInfo);

                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("transaction-status/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetTransactionStatus(string orderId)
        {
            try
            {
                var transaction = await _vnpayService.GetTransactionByOrderId(orderId);
                if (transaction == null)
                {
                    return NotFound(new { message = "Transaction not found" });
                }
                
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("user-transactions")]
        [Authorize]
        public async Task<IActionResult> GetUserTransactions()
        {
            try
            {
                // Get user ID from UserContext
                int userId = _userContext.UserID;
                
                var transactions = await _vnpayService.GetTransactionsByUserId(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("update-transaction")]
        [Authorize]
        public async Task<IActionResult> UpdateTransaction([FromBody] UpdateTransactionRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.OrderId))
                {
                    return BadRequest(new { message = "OrderId is required" });
                }

                if (string.IsNullOrEmpty(request.TransactionId))
                {
                    return BadRequest(new { message = "TransactionId is required" });
                }
                
                var transaction = await _vnpayService.UpdateTransactionIdAndDate(request.OrderId, request.TransactionId);
                
                if (transaction == null)
                {
                    return NotFound(new { message = "Transaction not found" });
                }
                
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
} 