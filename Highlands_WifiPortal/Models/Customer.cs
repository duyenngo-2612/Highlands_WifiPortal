using System.ComponentModel.DataAnnotations;

namespace Highlands_WifiPortal.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; } // Khớp với Database

        public string PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? ZaloUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
