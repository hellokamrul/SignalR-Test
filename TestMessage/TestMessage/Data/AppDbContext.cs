using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TestMessage.Models;

namespace TestMessage.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Messages> Messages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
