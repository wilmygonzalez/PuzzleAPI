using System;
using Microsoft.AspNetCore.Identity;

namespace PuzzleAPI.Data.Entities
{
	public class AppRole : IdentityRole<int>
	{
		public AppRole() : base() { }
        public AppRole(string roleName) : base(roleName) { }
    }
}

