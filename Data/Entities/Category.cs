using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuzzleAPI.Data.Entities
{
	public class Category : AppBaseEntity
	{
        [Required]
		public string Name { get; set; }
		public int? ParentCategoryId { get; set; }
        [ForeignKey("ParentCategoryId")]
		public virtual Category ParentCategory { get; set; }
		public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<CategoryRole> CategoryRoles { get; set; }
    }
}

