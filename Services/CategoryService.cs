using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PuzzleAPI.Data.Entities;
using PuzzleAPI.Extensions;
using PuzzleAPI.Interfaces;
using PuzzleAPI.Models;
using PuzzleAPI.Models.Category;

namespace PuzzleAPI.Services
{
	public class CategoryService : ICategoryService
    {
        #region Fields
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryRoleRepository _categoryRoleRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        #endregion

        #region Ctor
        public CategoryService(
            ICategoryRepository categoryRepository,
            ICategoryRoleRepository categoryRoleRepository,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
		{
			_categoryRepository = categoryRepository;
            _categoryRoleRepository = categoryRoleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        #region Utilities
        private List<TreeCategoryResponse> PrepareTreeCategory(List<TreeCategoryResponse> allCategories, int? parentCategoryId)
        {
            var categories = new List<TreeCategoryResponse>();
            var childCategories = allCategories.Where(x => x.ParentCategoryId == parentCategoryId).ToList();
            foreach(var category in childCategories)
            {
                var subCategories = PrepareTreeCategory(allCategories, category.CategoryId);
                category.ChildCategories.AddRange(subCategories);
                categories.Add(category);
            }

            return categories;
        }
        #endregion

        #region Methods
        public async Task<AppResponse<List<TreeCategoryResponse>>> GetCategories(string role)
        {
            var allCategories = await _categoryRepository.GetAll()
                .Include(x => x.ChildCategories)
                .Include(x => x.CategoryRoles)
                .Select(x => new TreeCategoryResponse
                {
                    CategoryId = x.Id,
                    Name = x.Name,
                    ParentCategoryId = x.ParentCategoryId,
                    CategoryRoles = x.CategoryRoles.Select(x => x.Role.Name).ToList()
                })
                .Where(x => x.CategoryRoles.Contains(role))
                .ToListAsync();

            var treeCategories = PrepareTreeCategory(allCategories, null);
            return AppResponse<List<TreeCategoryResponse>>.Valid(treeCategories, "Success");
        }

        public async Task<AppResponse> CreateCategoryAsync(CreateCategoryRequest request)
        {
            if (request.Roles.Count == 0)
                return AppResponse.Invalid("The category must have at least 1 role assigned.");

            // Verify if the parent category exists
            if (request.ParentCategoryId != null)
            {
                var parentCategoryExist = await _categoryRepository.Exists(request.ParentCategoryId ?? 0);
                if (!parentCategoryExist)
                    return AppResponse.Invalid("Parent Category doesn't exists.");
            }

            // Verify if the role exists
            var roles = new List<AppRole>();
            foreach(var roleName in request.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                    return AppResponse.Invalid($"The role {roleName} doesn't exists.");

                roles.Add(role);
            }

            // Insert the category
            var category = new Category
            {
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId
            };
            await _categoryRepository.InsertAsync(category);

            if (category.Id == 0)
                return AppResponse.Invalid("The category could't be created.");

            // Insert the category roles
            var categoryRoles = roles.Select(x => new CategoryRole
            {
                CategoryId = category.Id,
                RoleId = x.Id
            }).ToList();

            await _categoryRoleRepository.InsertAsync(categoryRoles);

            return AppResponse.Valid("Category created successfully");
        }

        public async Task<AppResponse> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            if (request.Roles.Count == 0)
                return AppResponse.Invalid("The category must have at least 1 role assigned.");

            // Verify if the parent category exists
            if (request.ParentCategoryId != null)
            {
                var parentCategoryExist = await _categoryRepository.Exists(request.ParentCategoryId ?? 0);
                if (!parentCategoryExist)
                    return AppResponse.Invalid("Parent Category doesn't exists.");
            }

            // Verify if the role exists
            var roles = new List<AppRole>();
            foreach (var roleName in request.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                    return AppResponse.Invalid($"The role {roleName} doesn't exists.");

                roles.Add(role);
            }

            // Verify if category exists
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (category == null)
                return AppResponse.Invalid("The category doesn't exists.");

            category.Name = request.Name;
            category.ParentCategoryId = request.ParentCategoryId;

            await _categoryRepository.UpdateAsync(category);

            // Update the category roles
            var oldCategoryRoles = await _categoryRoleRepository.GetAll().Where(x => x.CategoryId == category.Id).ToListAsync();
            await _categoryRoleRepository.DeleteRangeAsync(oldCategoryRoles);

            var categoryRoles = roles.Select(x => new CategoryRole
            {
                CategoryId = category.Id,
                RoleId = x.Id
            }).ToList();

            await _categoryRoleRepository.InsertAsync(categoryRoles);

            return AppResponse.Valid("Category updated successfully");
        }

        public async Task<AppResponse> DeleteCategoryAsync(int categoryId)
        {
            // Verify if category exists
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == categoryId);
            if (category == null)
                return AppResponse.Invalid("The category doesn't exists.");

            var categoryRoles = await _categoryRoleRepository.GetAll().Where(x => x.CategoryId == category.Id).ToListAsync();
            await _categoryRoleRepository.DeleteRangeAsync(categoryRoles);

            await _categoryRepository.DeleteAsync(category);

            return AppResponse.Valid("Category deleted successfully");
        }
        #endregion
    }
}

