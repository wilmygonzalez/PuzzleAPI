using System;
namespace PuzzleAPI.Models.Category
{
	public class TreeCategoryResponse
	{
		public string Name { get; set; }
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
		public List<string> CategoryRoles { get; set; } = new List<string>();
		public List<TreeCategoryResponse> ChildCategories { get; set; } = new List<TreeCategoryResponse>();
	}
}

