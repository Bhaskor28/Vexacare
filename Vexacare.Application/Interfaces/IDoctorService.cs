using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.UsersVM;


namespace Vexacare.Application.Users.Doctors
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorVM>> GetAllDoctorAsync();
        Task<DoctorVM> GetDoctorByIdAsync(string doctorId);
        Task<DoctorVM> AddDoctorAsync(DoctorVM doctor);
        Task<bool> DeleteDoctorAsync(string doctorId);
    }
}
