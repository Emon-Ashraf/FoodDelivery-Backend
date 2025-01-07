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
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _basketService.GetBasketAsync(userId);
            return Ok(result);
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
