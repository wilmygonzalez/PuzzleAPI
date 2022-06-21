using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PuzzleAPI.Data.Entities
{
	public class CategoryRole : AppBaseEntity
	{
		public int CategoryId { get; set; }
		public int RoleId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("RoleId")]
        public virtual AppRole Role { get; set; }
	}
}

