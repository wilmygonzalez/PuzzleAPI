using System;
using System.ComponentModel.DataAnnotations;

namespace PuzzleAPI.Models.User
{
	public class LoginTokenRequest
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}

