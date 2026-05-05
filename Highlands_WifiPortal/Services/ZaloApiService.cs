namespace Highlands_WifiPortal.Services
{
    public class ZaloApiService
    {
        //Giả lập để chạy code YC1 - Nhã sửa lại hàm đúng với yêu cầu 
        public async Task<bool> SendOtp(string phone, string otp)
        {
            // Giả lập độ trễ của mạng mất 1 giây để giao diện có cảm giác thật
            await Task.Delay(1000);

            return true; // Luôn báo gửi thành công để chạy tiếp sang màn hình Verify
        }
    }
}
