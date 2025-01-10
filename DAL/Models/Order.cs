using System;
using System.Collections.Generic;
using DTO.Enums;

namespace DAL.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.InProcess;
        public double Price { get; set; }
        public string Address { get; set; }

        public List<OrderDish> OrderDishes { get; set; }
    }

    public class OrderDish
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid DishId { get; set; }
        public Dish Dish { get; set; }

        public int Amount { get; set; }
        public double TotalPrice { get; set; }
    }
}
