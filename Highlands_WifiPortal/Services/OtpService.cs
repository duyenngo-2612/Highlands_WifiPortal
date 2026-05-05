namespace Highlands_WifiPortal.Services
{
    public class OtpService
    {
        //Giả lập để chạy code của yêu cầu 1.
        //Hoàn thiện lại code: Trình 
        public string GenerateOtp(string phone)
        {
            return "123456";
        }

        public void SaveOtp(string phone, string otp)
        {
        }

        public bool VerifyOtp(string phone, string otp)
        {
            if (otp == "123456")
            {
                return true;
            }
            return false;
        }
    }
}
