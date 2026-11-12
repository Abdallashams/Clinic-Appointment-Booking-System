namespace ClinicBookingSystem.Services.Abstraction;

    public interface ISpecialtyService
    {


     Task<IEnumerable<GetSpecialtyVM>> GetAllAsync();
     Task<GetSpecialtyVM?> GetByIdAsync(int id);
     Task<bool> CreateAsync(CreateSpecialtyVM model);
     Task<bool> UpdateAsync(UpdateSpecialtyVM model);
     Task<bool> DeleteAsync(int id);



}

