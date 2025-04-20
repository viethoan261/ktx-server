# Thông tin tích hợp VNPay

## Thông tin thẻ test

Dùng thông tin thẻ sau để test thanh toán VNPay trong môi trường sandbox:

| Thông tin | Giá trị |
|-----------|---------|
| Ngân hàng | NCB |
| Số thẻ | 9704198526191432198 |
| Tên chủ thẻ | NGUYEN VAN A |
| Ngày phát hành | 07/15 |
| Mật khẩu OTP | 123456 |

## API Documentation

### 1. Tạo thanh toán

```http
POST /api/Payment/create-payment
```

Body:
```json
{
  "amount": 10000,
  "orderDescription": "Thanh toán đơn hàng",
  "name": "Nguyen Van A",
  "orderType": "other",
  "orderId": "12345"
}
```

### 2. Lấy trạng thái giao dịch

```http
GET /api/Payment/transaction-status/{orderId}
```

### 3. Lấy lịch sử giao dịch 

```http
GET /api/Payment/user-transactions
```

### 4. Cập nhật thông tin giao dịch

```http
PUT /api/Payment/update-transaction
```

Body:
```json
{
  "orderId": "12345",
  "transactionId": "67890"
}
```

## Luồng thanh toán

1. Frontend gọi API `create-payment` để tạo thanh toán
2. Chuyển hướng người dùng đến URL thanh toán VNPay được trả về
3. Người dùng thực hiện thanh toán trên VNPay
4. Sau khi thanh toán, cập nhật trạng thái qua API `update-transaction`
5. Kiểm tra trạng thái giao dịch qua API `transaction-status/{orderId}`