using System;
using PuzzleAPI.Models;
using PuzzleAPI.Models.User;

namespace PuzzleAPI.Interfaces
{
	public interface IUserService
	{
        Task<AppResponse<TokenResponse>> LoginAsync(LoginTokenRequest request);
        Task<AppResponse> CreateUserAsync(CreateUserRequest request);
        Task<AppResponse> UpdateUserAsync(string currentUserEmail, UpdateUserRequest request);
        Task<AppResponse> DeleteUserAsync(string currentUserEmail, string email);
        Task<AppResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    }
}

