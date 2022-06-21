using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PuzzleAPI.Configurations;
using PuzzleAPI.Data.Entities;
using PuzzleAPI.Helpers;

namespace PuzzleAPI.Data
{
    public class AppDbSeeder
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        #region Ctor
        public AppDbSeeder(
            IOptions<AppSettings> appSettings,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        #endregion

        #region Utilities
        private List<string> GetAppRoles()
        {
            var roles = new List<string>
            {
                AppConstants.Roles.Admin,
                AppConstants.Roles.User,
                AppConstants.Roles.Public
            };

            return roles;
        }

        private async Task CreateRolesAsync()
        {
            var roles = GetAppRoles();
            foreach (var rol in roles)
            {
                var isRoleInDatabase = await _roleManager.FindByNameAsync(rol);
                if (isRoleInDatabase != null) continue;

                await _roleManager.CreateAsync(new AppRole(rol));
            }
        }

        private async Task CreateAdminUser()
        {
            var user = await _userManager.FindByEmailAsync(_appSettings.DefaultAdminEmail);
            if (user != null) return;

            user = new AppUser
            {
                FirstName = _appSettings.DefaultAdminFirstName,
                LastName = _appSettings.DefaultAdminLastName,
                Email = _appSettings.DefaultAdminEmail,
                UserName = _appSettings.DefaultAdminEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            await _userManager.CreateAsync(user, _appSettings.DefaultAdminPassword);
            await _userManager.AddToRoleAsync(user, AppConstants.Roles.Admin);
        }
        #endregion

        #region Methods
        public void InitializeDatabase()
        {
            Task.Run(async () =>
            {
                await CreateRolesAsync();
                await CreateAdminUser();
            }).GetAwaiter().GetResult();
        }
        #endregion
    }
}

