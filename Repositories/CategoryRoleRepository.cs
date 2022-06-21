using System;
using PuzzleAPI.Data;
using PuzzleAPI.Data.Entities;
using PuzzleAPI.Interfaces;

namespace PuzzleAPI.Repositories
{
	public class CategoryRoleRepository : AppRepository<CategoryRole>, ICategoryRoleRepository
    {
        #region Ctor
        public CategoryRoleRepository(AppDbContext dbContext) : base(dbContext) { }
        #endregion

        #region Methods

        #endregion
    }
}

