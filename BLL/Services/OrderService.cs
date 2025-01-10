using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using DTO;
using DTO.Enums;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderInfoDto>> GetOrdersAsync(Guid userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderInfoDto
                {
                    Id = o.Id,
                    DeliveryTime = o.DeliveryTime,
                    OrderTime = o.OrderTime,
                    Status = o.Status,
                    Price = o.Price
                })
                .ToListAsync();
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid userId, Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDishes)
                .ThenInclude(od => od.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            return new OrderDto
            {
                Id = order.Id,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Status = order.Status,
                Price = order.Price,
                Address = order.Address,
                Dishes = order.OrderDishes.Select(od => new DishBasketDto
                {
                    Id = od.DishId,
                    Name = od.Dish.Name,
                    Price = od.Dish.Price,
                    Amount = od.Amount,
                    TotalPrice = od.TotalPrice,
                    Image = od.Dish.Image
                }).ToList()
            };
        }

        public async Task<OrderDto> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto)
        {
            var basket = await _context.BasketItems
                .Include(bi => bi.Dish)
                .Where(bi => bi.UserId == userId)
                .ToListAsync();

            if (!basket.Any())
                throw new InvalidOperationException("Basket is empty.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DeliveryTime = orderCreateDto.DeliveryTime,
                Address = orderCreateDto.Address,
                Price = basket.Sum(b => b.Dish.Price * b.Amount),
                OrderDishes = basket.Select(b => new OrderDish
                {
                    DishId = b.DishId,
                    Amount = b.Amount,
                    TotalPrice = b.Dish.Price * b.Amount
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.BasketItems.RemoveRange(basket);
            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(userId, order.Id);
        }

        public async Task ConfirmOrderDeliveryAsync(Guid userId, Guid orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            order.Status = OrderStatus.Delivered;
            await _context.SaveChangesAsync();
        }
    }
}
