using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using DTO;
using DTO.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DishPagedListDto> GetDishesAsync(List<string> categories, bool? vegetarian, DishSorting sorting, int page)
        {
            var query = _context.Dishes.AsQueryable();

            if (categories != null && categories.Any())
            {
                var enumCategories = categories
                    .Select(c => Enum.TryParse<DishCategory>(c, true, out var result) ? result : (DishCategory?)null)
                    .Where(c => c.HasValue)
                    .Select(c => c.Value)
                    .ToList();

                query = query.Where(d => enumCategories.Contains(d.Category));
            }

            if (vegetarian.HasValue)
                query = query.Where(d => d.Vegetarian == vegetarian.Value);

            query = sorting switch
            {
                DishSorting.NameAsc => query.OrderBy(d => d.Name),
                DishSorting.NameDesc => query.OrderByDescending(d => d.Name),
                DishSorting.PriceAsc => query.OrderBy(d => d.Price),
                DishSorting.PriceDesc => query.OrderByDescending(d => d.Price),
                DishSorting.RatingAsc => query.OrderBy(d => d.Rating),
                DishSorting.RatingDesc => query.OrderByDescending(d => d.Rating),
                _ => query
            };

            const int pageSize = 5;
            var totalItems = await query.CountAsync();
            var dishes = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new DishPagedListDto
            {
                Dishes = dishes.Select(d => new DishDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    Image = d.Image,
                    Vegetarian = d.Vegetarian,
                    Rating = d.Rating,
                    Category = d.Category.ToString()
                }).ToList(),
                Pagination = new PageInfoModel
                {
                    Size = pageSize,
                    Count = (int)Math.Ceiling((double)totalItems / pageSize),
                    Current = page
                }
            };
        }

        public async Task<DishDto> GetDishByIdAsync(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) throw new KeyNotFoundException("Dish not found.");

            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Image = dish.Image,
                Vegetarian = dish.Vegetarian,
                Rating = dish.Rating,
                Category = dish.Category.ToString()
            };
        }
    }
}
