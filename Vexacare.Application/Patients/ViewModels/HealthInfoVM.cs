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
        [Required(ErrorMessage = "Height is required")]
        [Display(Name = "Height (cm)")]
        [Range(0.1, 300, ErrorMessage = "Height must be between 0.1 and 300 cm")]
        public decimal? Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Display(Name = "Weight (kg)")]
        [Range(0.1, 500, ErrorMessage = "Weight must be between 0.1 and 500 kg")]
        public decimal? Weight { get; set; }

        [Required(ErrorMessage = "BMI is required")]
        [Display(Name = "BMI")]
        [Range(0.1, 100, ErrorMessage = "BMI must be between 0.1 and 100")]
        public decimal? BMI { get; set; }

        // Current Medical Conditions
        [Required(ErrorMessage = "Main diagnoses is required")]
        [Display(Name = "Main Diagnoses")]
        [StringLength(500, ErrorMessage = "Main diagnoses cannot exceed 500 characters")]
        public string MainDiagnoses { get; set; } = string.Empty;

        [Required(ErrorMessage = "Diagnosis date is required")]
        [Display(Name = "Diagnosis Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DiagnosisDate { get; set; }

        // Drug Therapy Information
        [Required(ErrorMessage = "Drug name is required")]
        [Display(Name = "Drug Name")]
        [StringLength(100, ErrorMessage = "Drug name cannot exceed 100 characters")]
        public string DrugName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dosage is required")]
        [Display(Name = "Dosage")]
        [StringLength(50, ErrorMessage = "Dosage cannot exceed 50 characters")]
        public string Dosage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Frequency is required")]
        [Display(Name = "Frequency")]
        [StringLength(50, ErrorMessage = "Frequency cannot exceed 50 characters")]
        public string Frequency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
    }
}