using System.Collections.Generic;
using Highlands_WifiPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace Highlands_WifiPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OtpLog> OtpLogs { get; set; }
    }
}