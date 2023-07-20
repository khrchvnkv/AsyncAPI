using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ListeningRequest> ListeningRequests { get; set; }
    }
}