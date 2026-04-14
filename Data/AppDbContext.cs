using Microsoft.EntityFrameworkCore;
using TopUpMVC.Models;

namespace TopUpMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TopUp> TopUps { get; set; }
    }
}
