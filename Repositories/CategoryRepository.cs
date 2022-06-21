using System;
using PuzzleAPI.Data;
using PuzzleAPI.Data.Entities;
using PuzzleAPI.Interfaces;
using PuzzleAPI.Models.Category;

namespace PuzzleAPI.Repositories
{
	public class CategoryRepository : AppRepository<Category>, ICategoryRepository
    {
        #region Ctor
        public CategoryRepository(AppDbContext dbContext) : base(dbContext) { }
        #endregion

        #region Methods

        #endregion
    }
}

