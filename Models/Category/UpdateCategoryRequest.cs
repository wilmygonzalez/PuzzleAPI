using System;
using System.ComponentModel.DataAnnotations;

namespace PuzzleAPI.Models.Category
{
	public class UpdateCategoryRequest
	{
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}

