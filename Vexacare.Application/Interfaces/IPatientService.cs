using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Patients.ViewModels;

namespace Vexacare.Application.Interfaces
{
    public interface IPatientService
    {
        Task<BasicInfoVM> GetBasicInfoAsync(string patientId);
        Task<bool> SaveBasicInfoAsync(string patientId, BasicInfoVM modell);

        Task<HealthInfoVM> GetHealthInfoAsync(string patientId);
        Task<bool> SaveHealthInfoAsync(string patientId, HealthInfoVM model);

        Task<GastrointestinalInfoVM> GetGastrointestinalInfoAsync(string patientId);
        Task<bool> SaveGastrointestinalInfoAsync(string patientId, GastrointestinalInfoVM model);

        Task<SymptomsInfoVM> GetSymptomsInfoAsync(string patientId);
        Task<bool> SaveSymptomsInfoAsync(string patientId, SymptomsInfoVM model);

        Task<DietProfileInfoVM> GetDietProfileInfoAsync(string patientId);
        Task<bool> SaveDietProfileInfoAsync(string patientId, DietProfileInfoVM model);

        Task<LifestyleInfoVM> GetLifestyleInfoAsync(string patientId);
        Task<bool> SaveLifestyleInfoAsync(string patientId, LifestyleInfoVM model);

        Task<TherapiesInfoVM> GetTherapiesInfoAsync(string patientId);
        Task<bool> SaveTherapiesInfoAsync(string patientId, TherapiesInfoVM model);
    }
}
