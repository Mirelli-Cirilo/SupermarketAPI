using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Models;

namespace SupermarketAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new { Id = 1, Username = "Admin", Password = "Admin", Role = "Admin"},
                                                new { Id = 2, Username = "Inner", Password = "Inner", Role = "Inner" },
                                                new { Id = 3, Username = "Customer", Password = "Customer", Role = "Customer" });
        }
    }
}
