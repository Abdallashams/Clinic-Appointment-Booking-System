
namespace ClinicBookingSystem.Services.Abstraction
{
    public interface IUserService
    {
        Task<List<GetApplicationUserVM>> GetAllUsersAsync();

        Task<List<GetApplicationUserVM>> GetAllActiveUsersAsync();

        Task<GetApplicationUserVM?> GetUserByIdAsync(int userId);

        Task CreateUserAsync(RegisterVm user);

        Task LoginAsync(LoginVM user);
        Task SignOutAsync();

        Task DeleteUserAsync(int userId);
    }
}
