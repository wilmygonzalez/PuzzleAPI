using System;
using System.Security.Claims;
using PuzzleAPI.Data.Entities;

namespace PuzzleAPI.Interfaces
{
	public interface ITokenService
	{
        string GenerateRefreshToken();
        Task<string> GenerateJwtAsync(AppUser user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

