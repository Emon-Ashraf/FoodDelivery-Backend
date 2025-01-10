using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for OrderDish
            modelBuilder.Entity<OrderDish>()
                .HasKey(od => new { od.OrderId, od.DishId });

            // Define relationship between Order and OrderDish
            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDishes)
                .HasForeignKey(od => od.OrderId);

            // Define relationship between Dish and OrderDish
            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany()
                .HasForeignKey(od => od.DishId);
        }
    }
}
