using BLL.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishesController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDishes([FromQuery] List<string> categories, [FromQuery] bool? vegetarian, [FromQuery] string sorting, [FromQuery] int page = 1)
        {
            var result = await _dishService.GetDishesAsync(categories, vegetarian, sorting, page);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishById(Guid id)
        {
            var result = await _dishService.GetDishByIdAsync(id);
            return Ok(result);
        }
    }
}
