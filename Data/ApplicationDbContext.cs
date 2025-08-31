using Microsoft.EntityFrameworkCore;
using RI_App.Models;

namespace RI_App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // This will become a table in SQL Server
        public DbSet<ReportIssue> ReportIssues { get; set; }
    }
}
