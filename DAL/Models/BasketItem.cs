using System;
using DAL.Models;

namespace DAL.Models
{
    public class BasketItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DishId { get; set; }
        public int Amount { get; set; }
        public Dish Dish { get; set; } // Navigation property
    }
}
