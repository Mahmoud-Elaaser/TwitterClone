using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs.AuthenticationDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/user")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result == null)
                return BadRequest(new ApiResponse(result.Status, result.Message));

            return Ok(result);
        }

        [HttpPost("register/admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result == null)
                return BadRequest(new ApiResponse(result.Status, result.Message));

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.LoginAsync(model);
            if (result == null)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpGet("get-current-user")]
        public IActionResult GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { userId });
        }


        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] RoleDto model)
        {
            var result = await _authService.AssignRoleAsync(model);
            if (!result.IsSucceeded)
                return BadRequest(new ApiResponse(result.Status, result.Message));

            return Ok(result);
        }

        [HttpPost("remove-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleDto model)
        {
            var result = await _authService.RemoveRoleAsync(model);
            if (!result.IsSucceeded)
                return BadRequest(new ApiResponse(result.Status, result.Message));

            return Ok(result);
        }

        [HttpPost("update-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto model)
        {
            var result = await _authService.UpdateRoleAsync(model);
            if (!result.IsSucceeded)
                return BadRequest(new ApiResponse(result.Status, result.Message));

            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _authService.ChangePasswordAsync(model);
            if (!result.IsSucceeded)
                return BadRequest(new ApiResponse(result.Status, result.Message));

            return Ok(result);
        }

        //[HttpPost("generate-token-for-reset-pass")]
        //public async Task<IActionResult> GeneratePasswordResetToken([FromBody] string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return BadRequest("Email is required.");
        //    }

        //    var token = await _authService.GeneratePasswordResetTokenAsync(email);
        //    return Ok(new { Token = token });
        //}


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var response = _authService.SignOutAsync();
            return Ok(response);
        }

        /// TODO:
        /// 1. refresh token
        /// 2. Confirm email
        /// 3. resend confirm email
        /// 4. reset-passwod
        /// 5. forgot passwor
        /// 6. manage 2FA



    }
}