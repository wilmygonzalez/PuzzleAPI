using System;
using System.ComponentModel.DataAnnotations;

namespace PuzzleAPI.Models.Category
{
	public class CreateCategoryRequest
	{
        [Required]
		public string Name { get; set; }
		public int? ParentCategoryId { get; set; }
		public List<string> Roles { get; set; } = new List<string>();
	}
}

