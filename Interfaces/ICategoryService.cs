using System;
using PuzzleAPI.Data.Entities;
using PuzzleAPI.Extensions;
using PuzzleAPI.Models;
using PuzzleAPI.Models.Category;

namespace PuzzleAPI.Interfaces
{
	public interface ICategoryService
	{
        Task<AppResponse> CreateCategoryAsync(CreateCategoryRequest request);
        Task<AppResponse> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<AppResponse> DeleteCategoryAsync(int categoryId);
        Task<AppResponse<List<TreeCategoryResponse>>> GetCategories(string role);
    }
}

