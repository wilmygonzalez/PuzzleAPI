using System;
using System.ComponentModel.DataAnnotations;

namespace PuzzleAPI.Models.User
{
	public class CreateUserRequest
	{
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
    }
}

