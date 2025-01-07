using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _context;

        public BasketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DishBasketDto>> GetBasketAsync(Guid userId)
        {
            var basketItems = await _context.BasketItems
                .Include(b => b.Dish)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return basketItems.Select(b => new DishBasketDto
            {
                Id = b.Dish.Id,
                Name = b.Dish.Name,
                Price = b.Dish.Price,
                TotalPrice = b.Dish.Price * b.Amount,
                Amount = b.Amount,
                Image = b.Dish.Image
            }).ToList();
        }

        public async Task AddDishToBasketAsync(Guid userId, Guid dishId)
        {
            var basketItem = await _context.BasketItems
                .FirstOrDefaultAsync(b => b.UserId == userId && b.DishId == dishId);

            if (basketItem != null)
            {
                basketItem.Amount++;
            }
            else
            {
                _context.BasketItems.Add(new BasketItem
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DishId = dishId,
                    Amount = 1
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveDishFromBasketAsync(Guid userId, Guid dishId, bool increase)
        {
            var basketItem = await _context.BasketItems
                .FirstOrDefaultAsync(b => b.UserId == userId && b.DishId == dishId);

            if (basketItem == null)
                throw new KeyNotFoundException("Dish not found in basket.");

            if (increase)
            {
                basketItem.Amount--;
                if (basketItem.Amount == 0)
                    _context.BasketItems.Remove(basketItem);
            }
            else
            {
                _context.BasketItems.Remove(basketItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}
