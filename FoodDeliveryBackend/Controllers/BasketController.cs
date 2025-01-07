using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("User not authorized."));
                var result = await _basketService.GetBasketAsync(userId);
                if (result == null || result.Count == 0)
                {
                    return NotFound(new { status = "Basket is empty", message = "Please add dishes." });
                }
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { status = "Error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> AddDishToBasket(Guid dishId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _basketService.AddDishToBasketAsync(userId, dishId);
            return Ok();
        }

        [HttpDelete("dish/{dishId}")]
        public async Task<IActionResult> RemoveDishFromBasket(Guid dishId, [FromQuery] bool increase)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _basketService.RemoveDishFromBasketAsync(userId, dishId, increase);
            return Ok();
        }
    }
}
