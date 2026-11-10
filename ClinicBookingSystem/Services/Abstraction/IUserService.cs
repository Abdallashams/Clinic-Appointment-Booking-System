
namespace ClinicBookingSystem.Services.Abstraction
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();

        Task<List<ApplicationUser>> GetAllActiveUsersAsync();

        Task<ApplicationUser?> GetUserByIdAsync(int userId);

        Task CreateUserAsync(ApplicationUserVm user);

        Task UpdateUserAsync(ApplicationUserVm user);

        Task DeleteUserAsync(int userId);
    }
}
