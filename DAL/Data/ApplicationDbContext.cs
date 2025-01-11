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
        public DbSet<Rating> Ratings { get; set; }

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

            // Configure primary key for Rating
            modelBuilder.Entity<Rating>()
                .HasKey(r => r.Id);

            // Ensure a user can only rate a dish once
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.DishId })
                .IsUnique();

            // Define relationships for Rating
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Dish)
                .WithMany()
                .HasForeignKey(r => r.DishId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
