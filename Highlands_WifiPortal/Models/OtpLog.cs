using System;

namespace Highlands_WifiPortal.Models
{
    public class OtpLog
    {
        public int OtpId { get; set; }

        public string PhoneNumber { get; set; }

        public string OTPCode { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime ExpiredAt { get; set; }

        public bool IsUsed { get; set; }
    }
}