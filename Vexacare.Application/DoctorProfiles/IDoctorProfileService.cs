using Vexacare.Application.UsersVM;

namespace Vexacare.Application.DoctorProfiles
{
    public interface IDoctorProfileService
    {
        Task<IEnumerable<DoctorProfileVM>> GetAllDoctorProfiles();
        Task<DoctorProfileVM> GetDoctorProfileByIdAsync(int doctorId);
    }
}
