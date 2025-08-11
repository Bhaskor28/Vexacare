using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Patients.ViewModels
{
    public class HealthInfoVM
    {
        // Anthropometric Parameters
        [Display(Name = "Height (cm)")]
        public decimal? Height { get; set; }

        [Display(Name = "Weight (kg)")]
        public decimal? Weight { get; set; }

        [Display(Name = "BMI")]
        public decimal? BMI { get; set; }

        // Current Medical Conditions
        [Display(Name = "Main Diagnoses")]
        public string MainDiagnoses { get; set; }

        [Display(Name = "Diagnosis Date")]
        [DataType(DataType.Date)]
        public DateTime? DiagnosisDate { get; set; }

        // Drug Therapy Information
        [Required]
        [Display(Name = "Drug Name")]
        public string DrugName { get; set; }

        [Display(Name = "Dosage")]
        public string Dosage { get; set; }

        [Display(Name = "Frequency")]
        public string Frequency { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
    }
}
