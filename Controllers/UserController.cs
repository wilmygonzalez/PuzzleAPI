using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuzzleAPI.Helpers;
using PuzzleAPI.Interfaces;
using PuzzleAPI.Models.User;

namespace PuzzleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginTokenRequest request)
        {
            var response = await _userService.LoginAsync(request);
            return Ok(response);
        }

        [Authorize(Roles = AppConstants.Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var response = await _userService.CreateUserAsync(request);
            return Ok(response);
        }

        [Authorize(Roles = AppConstants.Roles.Admin)]
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UpdateUserRequest request)
        {
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var response = await _userService.UpdateUserAsync(currentUserEmail, request);
            return Ok(response);
        }

        [Authorize(Roles = AppConstants.Roles.Admin)]
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var response = await _userService.DeleteUserAsync(currentUserEmail, email);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response = await _userService.ForgotPasswordAsync(request);
            return Ok(response);
        }
    }
}

