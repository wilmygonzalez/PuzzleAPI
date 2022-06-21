using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PuzzleAPI.Configurations;
using PuzzleAPI.Data.Entities;
using PuzzleAPI.Helpers;
using PuzzleAPI.Interfaces;
using PuzzleAPI.Models;
using PuzzleAPI.Models.User;

namespace PuzzleAPI.Services
{
	public class UserService : IUserService
    {
        #region Fields
        private readonly AppSettings _appSettings;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        #endregion

        #region Ctor
        public UserService(
            IOptions<AppSettings> appSettings,
            ITokenService tokenService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _tokenService = tokenService;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        #region Methods
        public async Task<AppResponse<TokenResponse>> LoginAsync(LoginTokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return AppResponse<TokenResponse>.Invalid("User not found.");

            if (!user.EmailConfirmed && !user.PhoneNumberConfirmed)
                return AppResponse<TokenResponse>.Invalid("User not active. Please contact the administrator.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return AppResponse<TokenResponse>.Invalid("Invalid credentials.");

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_appSettings.TokenExpiresInDays);

            await _userManager.UpdateAsync(user);

            var token = await _tokenService.GenerateJwtAsync(user);
            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken
            };

            return AppResponse<TokenResponse>.Valid(response, $"Login successfully {DateTime.UtcNow}");
        }

        public async Task<AppResponse> CreateUserAsync(CreateUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
                return AppResponse.Invalid($"Email {request.Email} is already taken.");

            var isPublic = request.Role.Trim().ToString() == AppConstants.Roles.Public.Trim().ToLower();
            if (isPublic)
                return AppResponse.Invalid($"Cannot create an user with public role.");

            var validRole = await _roleManager.RoleExistsAsync(request.Role);
            if (!validRole)
                return AppResponse.Invalid($"The role {request.Role} is not valid.");

            user = new AppUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                EmailConfirmed = true
            };

            var userCreated = await _userManager.CreateAsync(user, request.Password);
            if (!userCreated.Succeeded)
                return AppResponse.Invalid($"User {request.Email} cannot be created.");

            await _userManager.AddToRoleAsync(user, request.Role);

            return AppResponse.Valid("User created successfully.");
        }

        public async Task<AppResponse> UpdateUserAsync(string currentUserEmail, UpdateUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return AppResponse.Invalid($"User {request.Email} doesn't exists.");

            if (currentUserEmail.Trim().ToLower() == _appSettings.DefaultAdminEmail.Trim().ToLower())
                return AppResponse.Invalid($"Cannot update the system admin.");

            if (currentUserEmail.Trim().ToLower() == user.Email.Trim().ToLower())
                return AppResponse.Invalid($"Cannot update the current user.");

            var isPublic = request.Role.Trim().ToString() == AppConstants.Roles.Public.Trim().ToLower();
            if (isPublic)
                return AppResponse.Invalid($"Cannot update an user with public role.");

            var validRole = await _roleManager.RoleExistsAsync(request.Role);
            if (!validRole)
                return AppResponse.Invalid($"The role {request.Role} is not valid.");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
                await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.AddToRoleAsync(user, request.Role);

            return AppResponse.Valid("User updated successfully.");
        }

        public async Task<AppResponse> DeleteUserAsync(string currentUserEmail, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return AppResponse.Invalid($"User {email} doesn't exists.");

            if (email.Trim().ToLower() == _appSettings.DefaultAdminEmail.Trim().ToLower())
                return AppResponse.Invalid($"Cannot delete the system admin.");

            if (currentUserEmail.Trim().ToLower() == email)
                return AppResponse.Invalid($"Cannot delete the current user.");

            await _userManager.DeleteAsync(user);

            return AppResponse.Valid("User deleted successfully.");
        }

        public async Task<AppResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return AppResponse.Invalid($"User {request.Email} doesn't exists.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
                return AppResponse.Invalid($"{string.Join(", ",result.Errors)}");

            return AppResponse.Valid("User password updated successfully.");
        }
        #endregion
    }
}

