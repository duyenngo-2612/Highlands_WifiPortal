using Highlands_WifiPortal.Data;
using Highlands_WifiPortal.Models;
using Highlands_WifiPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Highlands_WifiPortal.Controllers
{
    public class AuthController : Controller
    {
        // Khai báo các service
        private readonly OtpService _otpService;
        private readonly ZaloApiService _zaloApiService;
        private readonly ApplicationDbContext _context;

        public AuthController(OtpService otpService,ZaloApiService zaloApiService,ApplicationDbContext context)
        {
            _otpService = otpService;
            _zaloApiService = zaloApiService;
            _context = context;
        }

        // 1. Hiển thị màn hình Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Splash()
        {
            return View();
        }
        // 2. Xử lý khi người dùng bấm gửi OTP 
        [HttpPost]
        public async Task<IActionResult> SendOtp(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length != 10)
            {
                ViewBag.Error = "Vui lòng nhập số điện thoại hợp lệ.";
                return View("Login");
            }

            // BƯỚC 1: Gọi service để tạo mã OTP
            string otp = _otpService.GenerateOtp(phone);
            TempData["DemoOtp"] = otp;
            // BƯỚC 2: Gọi service của TRÌNH để lưu OTP vào database
            _otpService.SaveOtp(phone, otp);

            // BƯỚC 3: Gọi service để gửi OTP qua Zalo/SMS
            bool isSent = await _zaloApiService.SendOtp(phone, otp);

            if (isSent)
            {
                // BƯỚC 4: Chuyển sang màn hình nhập OTp
                return RedirectToAction("VerifyOtp", new { phone = phone });
            }
            else
            {
                ViewBag.Error = "Lỗi khi gửi OTP. Vui lòng thử lại.";
                return View("Login");
            }
        }

        // 3. Hiển thị màn hình nhập OTP
        [HttpGet]
        public IActionResult VerifyOtp(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return RedirectToAction("Login");
            }
            ViewBag.Phone = phone;
            return View();
        }

        // 4. Xử lý khi người dùng submit OTP 
        [HttpPost]
        public IActionResult VerifyOtp(string phone, string otp)
        {
            // BƯỚC 1: Gọi service của TRÌNH để kiểm tra OTP (đúng/sai, còn hạn, đã dùng chưa)
            bool isValid = _otpService.VerifyOtp(phone, otp);
            if (isValid)
            {
                HttpContext.Session.SetString("UserSession", phone);

                // tìm customer
                var customer = _context.Customers
                    .FirstOrDefault(x => x.PhoneNumber == phone);

                // tạo phiên truy cập
                var accessSession = new AccessSession
                {
                    CustomerId = customer.CustomerId,
                    MacAddress = "demo-mac",
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    LoginTime = DateTime.Now,
                    IsAuthenticated = true
                };

                _context.AccessSessions.Add(accessSession);
                _context.SaveChanges();

                return RedirectToAction("Success");
            }
            else
            {
                // BƯỚC 3: Nếu SAI -> Trả về thông báo lỗi
                ViewBag.Error = "Mã OTP không chính xác, đã hết hạn hoặc đã được sử dụng.";
                ViewBag.Phone = phone;
                return View();
            }
        }

        // 5. Hiển thị màn hình Thành công
        [HttpGet]
        public IActionResult Success()
        {
            // Kiểm tra bảo mật: Nếu chưa có session thì đẩy về Login
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserSession")))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult ZaloDemo(string otp)
        {
            ViewBag.Otp = otp;
            return View();
        }
    }
    }
