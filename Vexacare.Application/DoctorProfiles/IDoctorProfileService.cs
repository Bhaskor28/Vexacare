using Vexacare.Application.Products.ViewModels;
using Vexacare.Application.UsersVM;

namespace Vexacare.Application.DoctorProfiles
{
    public interface IDoctorProfileService
    {
        Task<IEnumerable<ProfileBasicVM>> GetAllDoctorProfiles();
        Task<ProfileBasicVM> GetDoctorProfileByIdAsync(int doctorId);
        Task<IEnumerable<ProfileBasicVM>> GetFilteredDoctorProfilesAsync(int? categoryId, int? serviceTypeId, int? locationId, int? availableId);
        Task CreateDoctorBasicProfile(ProfileBasicVM model);

        //added by sazib
        // Add these new methods for availability settings
        Task<DoctorSessionVM> GetDoctorSessionByUserIdAsync(string doctorId);
        Task<bool> SaveProfileSettingsAsync(DoctorSessionVM model);
        Task InitializeDefaultAvailabilityAsync(int doctorProfileId);
        Task<ProfileBasicVM> GetDoctorProfileByUserIdAsync(string doctorId);
    }
}
