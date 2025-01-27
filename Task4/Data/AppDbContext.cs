using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Task4.Models;

namespace Task4.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("AppDbContext"));
        }
        
        public DbSet<User> Users { get; set; }
    }
}