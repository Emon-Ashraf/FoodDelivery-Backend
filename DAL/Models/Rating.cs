using System;

namespace DAL.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } // Navigation property
        public Guid DishId { get; set; }
        public Dish Dish { get; set; } // Navigation property
        public int Score { get; set; } // Rating score (e.g., 0-10)
    }
}
