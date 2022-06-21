using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuzzleAPI.Helpers;
using PuzzleAPI.Interfaces;
using PuzzleAPI.Models.Category;

namespace PuzzleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = AppConstants.Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var response = await _categoryService.CreateCategoryAsync(request);
            return Ok(response);
        }

        [Authorize(Roles = AppConstants.Roles.Admin)]
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest request)
        {
            var response = await _categoryService.UpdateCategoryAsync(request);
            return Ok(response);
        }

        [Authorize(Roles = AppConstants.Roles.Admin)]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var response = await _categoryService.DeleteCategoryAsync(categoryId);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? AppConstants.Roles.Public;
            var response = await _categoryService.GetCategories(userRole);
            return Ok(response);
        }
    }
}

