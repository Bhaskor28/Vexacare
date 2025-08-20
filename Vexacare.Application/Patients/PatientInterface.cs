using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Patients
{
    public interface PatientInterface
    {
        Task GetDoctorById(string patientId);
        Task GetPatientById(string patientId);
        //Task GetAllDoctors(Patient patientId);
        Task GetAllPatient(string patientId);
    }
}
