using System.ComponentModel.DataAnnotations;

namespace Highlands_WifiPortal.Models
{
    public class AccessSession
    {
        [Key]
        public int SessionId { get; set; }

        public int CustomerId { get; set; }

        public string MacAddress { get; set; }

        public string? IPAddress { get; set; }

        public DateTime? LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public bool? IsAuthenticated { get; set; }
    }
}
