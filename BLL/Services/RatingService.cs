using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanUserRateDishAsync(Guid userId, Guid dishId)
        {
            var orderExists = await _context.Orders
                .Include(o => o.OrderDishes)
                .AnyAsync(o => o.UserId == userId && o.OrderDishes.Any(od => od.DishId == dishId));

            return orderExists;
        }

        public async Task SetRatingAsync(Guid userId, Guid dishId, int score)
        {
            if (score < 0 || score > 10)
                throw new ArgumentOutOfRangeException(nameof(score), "Rating must be between 0 and 10.");

            // Ensure the dish exists
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                throw new ArgumentException("Dish not found.", nameof(dishId));

            // Ensure the user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.", nameof(userId));

            // Check for existing rating
            var existingRating = await _context.Ratings.FirstOrDefaultAsync(r => r.UserId == userId && r.DishId == dishId);

            if (existingRating != null)
            {
                existingRating.Score = score;
            }
            else
            {
                var rating = new Rating
                {
                    UserId = userId,
                    DishId = dishId,
                    Score = score
                };
                _context.Ratings.Add(rating);
            }

            // Save changes and update dish rating
            await _context.SaveChangesAsync();
            await UpdateDishRatingAsync(dishId);
        }

        private async Task UpdateDishRatingAsync(Guid dishId)
        {
            var averageRating = await _context.Ratings
                .Where(r => r.DishId == dishId)
                .AverageAsync(r => r.Score);

            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish != null)
            {
                dish.Rating = averageRating;
                await _context.SaveChangesAsync();
            }
        }
    }
}
