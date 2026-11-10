
using Microsoft.AspNetCore.Identity;

namespace ClinicBookingSystem.Services.Implementation
{
    
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task CreateUserAsync(ApplicationUserVm user)
        {
            var newUser = new ApplicationUser
            {
                FullName = user.FullName,
                UserName = user.Email,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive ?? true
            };

            var result = await userManager.CreateAsync(newUser,user.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new Exception($"Failed to create user: {errors}");
            }
        }
        public Task DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUser>> GetAllActiveUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(ApplicationUserVm user)
        {
            throw new NotImplementedException();
        }
    }
}
