using BLL.Interfaces;
using DTO;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;

        public OrderController(IOrderService orderService, IBasketService basketService)
        {
            _orderService = orderService;
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { status = "Error", message = "User not authorized." });
                }

                var userId = Guid.Parse(userIdClaim);
                var orders = await _orderService.GetOrdersAsync(userId);

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { status = "Error", message = "User not authorized." });
                }

                var userId = Guid.Parse(userIdClaim);
                var order = await _orderService.GetOrderByIdAsync(userId, id);

                if (order == null)
                {
                    return NotFound(new { status = "Error", message = "Order not found." });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { status = "Error", message = "User not authorized." });
                }

                var userId = Guid.Parse(userIdClaim);
                var basketItems = await _basketService.GetBasketAsync(userId);

                if (basketItems == null || basketItems.Count == 0)
                {
                    return BadRequest(new { status = "Error", message = "Basket is empty." });
                }

                var order = await _orderService.CreateOrderAsync(userId, orderCreateDto);
                return Ok(new { status = "Success", message = "Order created successfully.", data = order });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> ConfirmOrderDelivery(Guid id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { status = "Error", message = "User not authorized." });
                }

                var userId = Guid.Parse(userIdClaim);
                await _orderService.ConfirmOrderDeliveryAsync(userId, id);

                return Ok(new { status = "Success", message = "Order delivery confirmed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
