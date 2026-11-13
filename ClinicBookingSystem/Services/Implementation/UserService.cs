
using ClinicBookingSystem.ModelVM.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem.Services.Implementation
{
    
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task CreateUserAsync(RegisterVm user)
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

            var roleResult = await userManager.AddToRoleAsync(newUser, "Patient");


            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new Exception($"Failed to create user: {errors}");
            }
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            user.IsActive = false;

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }



        public async Task<List<GetApplicationUserVM>> GetAllUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            if (users == null || users.Count == 0)
            {
                throw new Exception("No users found.");
            }
            var userVM = users.Select(u => new GetApplicationUserVM
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive
            }).ToList();
            return (userVM);
        }

        public async Task<SignInResult> LoginAsync(LoginVM user)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
                return SignInResult.Failed;

            var result = await signInManager.PasswordSignInAsync(
                existingUser,
                user.Password,
                isPersistent: true,
                lockoutOnFailure: true
            );

            return result;
        }

        public async Task SignOutAsync()
        {
 
            await signInManager.SignOutAsync();
          }



        public async Task<List<GetApplicationUserVM>> GetAllActiveUsersAsync()
        {
            var users = await userManager.Users.Where(e => e.IsActive==true).ToListAsync();
            if (users == null || users.Count == 0)
            {
                throw new Exception("No users found.");
            }
            var userVM = users.Select(u => new GetApplicationUserVM
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive
            }).ToList();
            return (userVM);
        }

        public async Task<GetApplicationUserVM?> GetUserByIdAsync(int userId)
        {
            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());

            if (user == null)
            {
                throw new Exception($"User with ID {userId} not found.");
            }

            return new GetApplicationUserVM
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

        }
        public async Task<bool> AddUserToAdmin(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
                return false;

            var result = await userManager.AddToRoleAsync(user, "Admin");

            return result.Succeeded;
        }


        public async Task<bool> AddUserToDoctor(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var isAdmin = await userManager.IsInRoleAsync(user, "Doctor");

            if (isAdmin)
                return false;

            var result = await userManager.AddToRoleAsync(user, "Doctor");

            return result.Succeeded;
        }
    }
}
