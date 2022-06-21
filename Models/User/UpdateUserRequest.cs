using System;
using System.ComponentModel.DataAnnotations;

namespace PuzzleAPI.Models.User
{
	public class UpdateUserRequest
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
		public string Role { get; set; }
	}
}

