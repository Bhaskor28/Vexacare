using System.ComponentModel.DataAnnotations;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class HealthInfo
    {
        public int Id { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal BMI { get; set; }
        public string MainDiagnoses { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public string DrugName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public string PatientId { get; set; } // Foreign key to Patient/IdentityUser
        public ApplicationUser Patient { get; set; }

    }
}
