using Vexacare.Application.UsersVM;

namespace Vexacare.Application.DoctorProfiles
{
    public interface IDoctorProfileService
    {
        Task<IEnumerable<DoctorProfileVM>> GetAllDoctorProfiles();
        Task<DoctorProfileVM> GetDoctorProfileByIdAsync(int doctorId);
        Task<IEnumerable<DoctorProfileVM>> GetFilteredDoctorProfilesAsync(int? categoryId, int? serviceTypeId, int? locationId, int? availableId);
    }
}
