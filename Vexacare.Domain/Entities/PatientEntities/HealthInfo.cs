using System.ComponentModel.DataAnnotations;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class HealthInfo
    {
        public int Id { get; set; }

       // Anthropometric Parameters
        [Display(Name = "Height (cm)")]
        public decimal? Height { get; set; }

        [Display(Name = "Weight (kg)")]
        public decimal? Weight { get; set; }

        [Display(Name = "BMI")]
        public decimal? BMI { get; set; }

        // Current Medical Conditions
        [Display(Name = "Main Diagnoses")]
        [StringLength(500)]
        public string MainDiagnoses { get; set; }

        [Display(Name = "Diagnosis Date")]
        [DataType(DataType.Date)]
        public DateTime? DiagnosisDate { get; set; }

        //Drug Info
        [Required]
        [Display(Name = "Drug Name")]
        [StringLength(100)]
        public string DrugName { get; set; }

        [StringLength(100)]
        public string Dosage { get; set; }

        [StringLength(100)]
        public string Frequency { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }


        [Required]
        public string PatientId { get; set; } // Foreign key to Patient/IdentityUser

        // Navigation property
        public Patient Patient { get; set; }

    }
}
