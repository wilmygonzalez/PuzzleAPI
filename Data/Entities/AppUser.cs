using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PuzzleAPI.Data.Entities
{
	public class AppUser : IdentityUser<int>
	{
		public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

