using System;
using System.ComponentModel.DataAnnotations;

namespace Highlands_WifiPortal.Models
{
    public class OtpLog
    {
        [Key]
        public int OtpId { get; set; }

        public string PhoneNumber { get; set; }
        public int CustomerId { get; set; } 

        public string OTPCode { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime ExpiredAt { get; set; }
        public string Channel { get; set; }

        public bool? IsUsed { get; set; }

    }

}