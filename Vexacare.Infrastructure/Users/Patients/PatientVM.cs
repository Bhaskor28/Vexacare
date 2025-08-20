using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Application.Users.Patients
{
    public class PatientVM
    {
        public string PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        // Related Entities
        public BasicInfo BasicInfo { get; set; }
        public HealthInfo HealthInfo { get; set; }
        public DietProfileInfo DietProfileInfo { get; set; }
        public GastrointestinalInfo GastrointestinalInfo { get; set; }
        public LifestyleInfo LifestyleInfo { get; set; }
        public SymptomsInfo SymptomsInfo { get; set; }
        public TherapiesInfo TherapiesInfo { get; set; }

        // Optionally add lists if relations are One-to-Many
        public List<HealthInfo> HealthHistory { get; set; } = new();
        public List<TherapiesInfo> TherapiesHistory { get; set; } = new();
    }
}
