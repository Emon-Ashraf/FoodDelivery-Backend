using BLL.Interfaces;
using DAL.Models;
using DTO;
using DTO.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;
        private readonly IRatingService _ratingService;

        public DishesController(IDishService dishService, IRatingService ratingService)
        {
            _dishService = dishService;
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDishes([FromQuery] DishFilterModel filter)
        {
            try
            {
                var categories = filter.Categories.Select(c => c.ToString()).ToList();
                var result = await _dishService.GetDishesAsync(categories, filter.Vegetarian, filter.Sorting, filter.Page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An error occurred while fetching dishes.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishById(Guid id)
        {
            try
            {
                var result = await _dishService.GetDishByIdAsync(id);
                return result != null ? Ok(result) : NotFound(new { status = "Error", message = "Dish not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An error occurred while fetching the dish.", details = ex.Message });
            }
        }

        [HttpGet("{id}/rating/check")]
        public async Task<IActionResult> CanUserRateDish(Guid id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { status = "Error", message = "User not authorized." });

                var userId = Guid.Parse(userIdClaim);
                var canRate = await _ratingService.CanUserRateDishAsync(userId, id);
                return Ok(canRate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An error occurred while checking rating eligibility.", details = ex.Message });
            }
        }

        [HttpPost("{id}/rating")]
        public async Task<IActionResult> SetDishRating(Guid id, [FromQuery] int ratingScore)
        {
            try
            {
                if (ratingScore < 0 || ratingScore > 10)
                    return BadRequest(new { status = "Error", message = "Rating score must be between 0 and 10." });

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { status = "Error", message = "User not authorized." });

                var userId = Guid.Parse(userIdClaim);
                await _ratingService.SetRatingAsync(userId, id, ratingScore);
                return Ok(new { status = "Success", message = "Rating submitted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An error occurred while submitting the rating.", details = ex.Message });
            }
        }
    }
}
