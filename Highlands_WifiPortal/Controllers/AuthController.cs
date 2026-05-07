using Highlands_WifiPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Highlands_WifiPortal.Controllers
{
    public class AuthController : Controller
    {
        // Khai báo các service của Trình và Nhã để sử dụng (Dependency Injection)
        private readonly OtpService _otpService;
        private readonly ZaloApiService _zaloApiService;

        public AuthController(OtpService otpService, ZaloApiService zaloApiService)
        {
            _otpService = otpService;
            _zaloApiService = zaloApiService;
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
        // 2. Xử lý khi người dùng bấm gửi OTP (Nhiệm vụ của Duyên)
        [HttpPost]
        public async Task<IActionResult> SendOtp(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                ViewBag.Error = "Vui lòng nhập số điện thoại hợp lệ.";
                return View("Login");
            }

            // BƯỚC 1: Gọi service của TRÌNH để tạo mã OTP
            string otp = _otpService.GenerateOtp(phone);

            // BƯỚC 2: Gọi service của TRÌNH để lưu OTP vào database
            _otpService.SaveOtp(phone, otp);

            // BƯỚC 3: Gọi service của NHÃ để gửi OTP qua Zalo/SMS
            bool isSent = await _zaloApiService.SendOtp(phone, otp);

            if (isSent)
            {
                // BƯỚC 4: Chuyển sang màn hình nhập OTP (Nhiệm vụ Duyên)
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

        // 4. Xử lý khi người dùng submit OTP (Nhiệm vụ của Duyên)
        [HttpPost]
        public IActionResult VerifyOtp(string phone, string otp)
        {
            // BƯỚC 1: Gọi service của TRÌNH để kiểm tra OTP (đúng/sai, còn hạn, đã dùng chưa)
            bool isValid = _otpService.VerifyOtp(phone, otp);

            if (isValid)
            {
                // BƯỚC 2: Nếu ĐÚNG -> Tạo phiên truy cập mạng (Session)
                HttpContext.Session.SetString("UserSession", phone);

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
    }
    }
