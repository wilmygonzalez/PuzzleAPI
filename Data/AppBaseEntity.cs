using System;
using System.ComponentModel.DataAnnotations;

namespace PuzzleAPI.Data
{
	public class AppBaseEntity
	{
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; }
    }
}

