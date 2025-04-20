using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Config;
using WebFilm.Core.Enitites.Payment;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly VNPayConfig _vnpayConfig;
        private readonly ITransactionRepository _transactionRepository;

        public VNPayService(IOptions<VNPayConfig> vnpayConfig, ITransactionRepository transactionRepository)
        {
            _vnpayConfig = vnpayConfig.Value;
            _transactionRepository = transactionRepository;
        }

        public string CreatePaymentUrl(HttpContext context, VNPayPaymentRequestDTO model)
        {
            string vnp_Version = "2.1.0";
            string vnp_Command = "pay";
            string vnp_TxnRef = model.OrderId;
            string vnp_IpAddr = GetIpAddress(context);
            string vnp_TmnCode = _vnpayConfig.TmnCode;
            string orderType = model.OrderType ?? "other";

            var vnp_Params = new Dictionary<string, string>();
            vnp_Params.Add("vnp_Version", vnp_Version);
            vnp_Params.Add("vnp_Command", vnp_Command);
            vnp_Params.Add("vnp_TmnCode", vnp_TmnCode);
            vnp_Params.Add("vnp_Amount", ((int)model.Amount * 100).ToString());
            vnp_Params.Add("vnp_CurrCode", "VND");
            vnp_Params.Add("vnp_TxnRef", vnp_TxnRef);
            vnp_Params.Add("vnp_OrderInfo", $"{model.Name} thanh toán: {model.OrderDescription}");
            vnp_Params.Add("vnp_OrderType", orderType);
            vnp_Params.Add("vnp_Locale", "vn");
            
            var urlReturn = model.ReturnUrl ?? _vnpayConfig.ReturnUrl;
            vnp_Params.Add("vnp_ReturnUrl", urlReturn);
            vnp_Params.Add("vnp_IpAddr", vnp_IpAddr);

            // Tạo vnp_CreateDate
            DateTime cld = DateTime.Now.ToUniversalTime().AddHours(7);
            string vnp_CreateDate = cld.ToString("yyyyMMddHHmmss");
            vnp_Params.Add("vnp_CreateDate", vnp_CreateDate);

            // Tạo vnp_ExpireDate: thêm 15 phút
            DateTime expireDate = cld.AddMinutes(15);
            string vnp_ExpireDate = expireDate.ToString("yyyyMMddHHmmss");
            vnp_Params.Add("vnp_ExpireDate", vnp_ExpireDate);
            
            // Tạo vnp_SecureHash
            var fieldNames = new List<string>(vnp_Params.Keys);
            fieldNames.Sort();
            
            var hashData = new StringBuilder();
            var query = new StringBuilder();
            
            foreach (var fieldName in fieldNames)
            {
                string fieldValue = vnp_Params[fieldName];
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    // Build hash data
                    hashData.Append(fieldName);
                    hashData.Append('=');
                    hashData.Append(WebUtility.UrlEncode(fieldValue));
                    
                    // Build query
                    query.Append(WebUtility.UrlEncode(fieldName));
                    query.Append('=');
                    query.Append(WebUtility.UrlEncode(fieldValue));
                    
                    if (fieldNames.IndexOf(fieldName) < fieldNames.Count - 1)
                    {
                        query.Append('&');
                        hashData.Append('&');
                    }
                }
            }
            
            string queryUrl = query.ToString();
            string vnp_SecureHash = HmacSHA512Helper(_vnpayConfig.HashSecret, hashData.ToString());
            queryUrl += "&vnp_SecureHash=" + vnp_SecureHash;
            
            string paymentUrl = _vnpayConfig.PaymentUrl + "?" + queryUrl;
            return paymentUrl;
        }

        public VNPayPaymentResponseDTO ProcessPaymentResponse(IQueryCollection collection)
        {
            var vnpayData = new Dictionary<string, string>();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpayData.Add(key, value);
                }
            }

            var orderId = vnpayData.ContainsKey("vnp_TxnRef") ? vnpayData["vnp_TxnRef"] : "";
            var vnpResponseCode = vnpayData.ContainsKey("vnp_ResponseCode") ? vnpayData["vnp_ResponseCode"] : "";
            var vnpTransactionId = vnpayData.ContainsKey("vnp_TransactionNo") ? vnpayData["vnp_TransactionNo"] : "";
            var vnpSecureHash = collection.ContainsKey("vnp_SecureHash") ? collection["vnp_SecureHash"].ToString() : "";
            var orderInfo = vnpayData.ContainsKey("vnp_OrderInfo") ? vnpayData["vnp_OrderInfo"] : "";
            var paymentMethod = "VNPay";

            var checkSignature = ValidateSignature(vnpSecureHash, _vnpayConfig.HashSecret, vnpayData);

            var success = checkSignature && vnpResponseCode == "00";

            return new VNPayPaymentResponseDTO
            {
                Success = success,
                PaymentMethod = paymentMethod,
                OrderDescription = orderInfo,
                OrderId = orderId,
                PaymentId = vnpTransactionId,
                TransactionId = vnpTransactionId,
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode
            };
        }

        private bool ValidateSignature(string inputHash, string secretKey, Dictionary<string, string> requestData)
        {
            var rspRaw = GetResponseData(requestData);
            var myChecksum = HmacSHA512Helper(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData(Dictionary<string, string> requestData)
        {
            var data = new StringBuilder();
            if (requestData.ContainsKey("vnp_SecureHashType"))
            {
                requestData.Remove("vnp_SecureHashType");
            }

            if (requestData.ContainsKey("vnp_SecureHash"))
            {
                requestData.Remove("vnp_SecureHash");
            }

            // Sắp xếp các tham số theo tên
            var fieldNames = new List<string>(requestData.Keys);
            fieldNames.Sort();

            foreach (var fieldName in fieldNames)
            {
                string fieldValue = requestData[fieldName];
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    data.Append(WebUtility.UrlEncode(fieldName) + "=" + WebUtility.UrlEncode(fieldValue));
                    if (fieldNames.IndexOf(fieldName) < fieldNames.Count - 1)
                    {
                        data.Append("&");
                    }
                }
            }

            return data.ToString();
        }

        public async Task<Transaction> SaveTransaction(VNPayPaymentRequestDTO request, int userId)
        {
            var transaction = new Transaction
            {
                OrderId = request.OrderId,
                UserId = userId,
                Amount = request.Amount,
                OrderDescription = request.OrderDescription,
                Status = "PENDING",
                CreatedDate = DateTime.Now
            };

            var result = await _transactionRepository.CreateTransaction(transaction);
            return transaction;
        }

        public async Task<Transaction> UpdateTransactionStatus(string orderId, string status, string transactionId)
        {
            await _transactionRepository.UpdateTransactionStatus(orderId, status, transactionId);
            return await _transactionRepository.GetByOrderId(orderId);
        }

        public async Task<Transaction> GetTransactionByOrderId(string orderId)
        {
            return await _transactionRepository.GetByOrderId(orderId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserId(int userId)
        {
            return await _transactionRepository.GetByUserId(userId);
        }

        public async Task<Transaction> UpdateTransactionIdAndDate(string orderId, string transactionId)
        {
            await _transactionRepository.UpdateTransactionIdAndDate(orderId, transactionId);
            return await _transactionRepository.GetByOrderId(orderId);
        }

        private string GetIpAddress(HttpContext context)
        {
            string ipAddress;
            try
            {
                ipAddress = context.Connection.RemoteIpAddress.ToString();

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress == "::1"))
                {
                    ipAddress = "127.0.0.1";
                }
            }
            catch (Exception ex)
            {
                ipAddress = "127.0.0.1";
            }

            return ipAddress;
        }

        private string HmacSHA512Helper(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
    }
} 