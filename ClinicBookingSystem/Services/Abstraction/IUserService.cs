
using ClinicBookingSystem.ModelVM.User;
using Microsoft.AspNetCore.Identity;

namespace ClinicBookingSystem.Services.Abstraction
{
    public interface IUserService
    {
        Task<List<GetApplicationUserVM>> GetAllUsersAsync();

        Task<List<GetApplicationUserVM>> GetAllActiveUsersAsync();

        Task<GetApplicationUserVM?> GetUserByIdAsync(int userId);

        Task CreateUserAsync(RegisterVm user);

        public Task<SignInResult> LoginAsync(LoginVM user);
        Task SignOutAsync();

        public Task<bool> DeleteUserAsync(string userId);
        public Task<bool> AddUserToAdmin(string userId);
        public Task<bool> AddUserToDoctor(string userId);


    }
}
