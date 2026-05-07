using System;
using System.Linq;
using Highlands_WifiPortal.Data;
using Highlands_WifiPortal.Models;

namespace Highlands_WifiPortal.Services
{
    public class OtpService
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random = new Random();

        public OtpService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Generate OTP 6 số
        public string GenerateOtp(string phone)
        {
            return _random.Next(100000, 999999).ToString();
        }

        // 2. Lưu OTP vào DB
        public void SaveOtp(string phone, string otp)
        {
            // Bước A: Tìm xem khách hàng này có trong DB chưa
            var customer = _context.Customers.FirstOrDefault(c => c.PhoneNumber == phone);

            // Bước B: Nếu là khách mới, tạo luôn bản ghi Customer
            if (customer == null)
            {
                customer = new Customer
                {
                    PhoneNumber = phone,
                    CreatedAt = DateTime.Now
                };
                _context.Customers.Add(customer);
                _context.SaveChanges(); // Lưu để SQL tự sinh ra CustomerId
            }

            // Bước C: Lưu OTP Log với ID Khách hàng hợp lệ
            var otpLog = new OtpLog
            {
                CustomerId = customer.CustomerId, // Dùng ID vừa lấy được
                PhoneNumber = phone,
                OTPCode = otp,
                SentAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddMinutes(3),
                IsUsed = false,
                Channel = "Zalo" // Có thể đổi thành SMS tùy logic
            };

            _context.OtpLogs.Add(otpLog);
            _context.SaveChanges();
        }

        // 3. Verify OTP
        public bool VerifyOtp(string phone, string otp)
        {

            var record = _context.OtpLogs
                .Where(x => x.PhoneNumber == phone && x.OTPCode == otp)
                .OrderByDescending(x => x.SentAt)
                .FirstOrDefault();

            if (record == null)
                return false;

            // kiểm tra hết hạn
            if (record.ExpiredAt < DateTime.Now)
                return false;

            // kiểm tra đã dùng chưa
            if (record.IsUsed==true)
                return false;

            // đúng thì xác nhận thành công và đánh dấu đã dùng
            record.IsUsed = true;
            _context.SaveChanges();

            return true;
        }
    }
}