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
            var otpLog = new OtpLog
            {
                PhoneNumber = phone,
                OTPCode = otp,
                SentAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddMinutes(3),
                IsUsed = false
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
            if (record.IsUsed)
                return false;

            // đúng thì xác nhận thành công và đánh dấu đã dùng
            record.IsUsed = true;
            _context.SaveChanges();

            return true;
        }
    }
}