using BLL.Interfaces;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize] // Ensure all endpoints (except [AllowAnonymous]) require authentication
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST /api/account/register
        [HttpPost("register")]
        [AllowAnonymous] // Allow unauthenticated users to register
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid registration data" });
            }

            var token = await _userService.RegisterUserAsync(model);
            return Ok(new { token });
        }

        // POST /api/account/login
        [HttpPost("login")]
        [AllowAnonymous] // Allow unauthenticated users to log in
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid login credentials" });
            }

            try
            {
                var token = await _userService.LoginAsync(credentials);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // POST /api/account/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _userService.LogoutAsync();
            return Ok(new { message = "Logged out successfully" });
        }

        // GET /api/account/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var profile = await _userService.GetUserProfileAsync(userId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // PUT /api/account/profile
        [HttpPut("profile")]
        public async Task<IActionResult> EditProfile([FromBody] UserEditModel model)
        {
            try
            {
                var userId = GetUserIdFromToken();
                await _userService.EditUserProfileAsync(userId, model);
                return Ok(new { message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Helper method to extract userId from JWT token
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return Guid.Parse(userIdClaim.Value);
        }
    }
}
