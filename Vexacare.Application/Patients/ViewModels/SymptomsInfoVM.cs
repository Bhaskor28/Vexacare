using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Patients.ViewModels
{
    public class SymptomsInfoVM
    {
        // Intestinal Function
        [Required(ErrorMessage = "Frequency of evacuations is required")]
        [Display(Name = "Frequency of evacuations")]
        [Range(0, 50, ErrorMessage = "Frequency must be between 0 and 50")]
        public int? FrequencyOfEvaluations { get; set; }

        [Required(ErrorMessage = "Bristol Scale is required")]
        [Display(Name = "Bristol Scale")]
        [Range(1, 7, ErrorMessage = "Bristol Scale must be between 1 and 7")]
        public int? BristolScale { get; set; }

        // Current Symptoms (0-10 scales)
        [Required(ErrorMessage = "Bloating Severity is required")]
        [Display(Name = "Bloating Severity")]
        [Range(0, 10, ErrorMessage = "Bloating Severity must be between 0 and 10")]
        public int? BloatingSeverity { get; set; }

        [Required(ErrorMessage = "Intestinal Gas is required")]
        [Display(Name = "Intestinal Gas")]
        [Range(0, 10, ErrorMessage = "Intestinal Gas must be between 0 and 10")]
        public int? IntestinalGas { get; set; }

        [Required(ErrorMessage = "Abdominal Pain is required")]
        [Display(Name = "Abdominal Pain")]
        [Range(0, 10, ErrorMessage = "Abdominal Pain must be between 0 and 10")]
        public int? AbdominalPain { get; set; }

        [Required(ErrorMessage = "Digestive Difficulties is required")]
        [Display(Name = "Digestive Difficulties")]
        [Range(0, 10, ErrorMessage = "Digestive Difficulties must be between 0 and 10")]
        public int? DigestiveDifficulties { get; set; }

        // Food Sensitivities
        [Required(ErrorMessage = "Diagnosed Intolerances is required")]
        [Display(Name = "Diagnosed Intolerances")]
        [StringLength(255, ErrorMessage = "Diagnosed Intolerances cannot exceed 255 characters")]
        public string DiagnosedIntolerances { get; set; } = string.Empty;

        [Required(ErrorMessage = "Certified Allergies is required")]
        [Display(Name = "Certified Allergies")]
        [StringLength(255, ErrorMessage = "Certified Allergies cannot exceed 255 characters")]
        public string CertifiedAllergies { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tests Performed is required")]
        [Display(Name = "Tests Performed")]
        [StringLength(255, ErrorMessage = "Tests Performed cannot exceed 255 characters")]
        public string TestsPerformed { get; set; } = string.Empty;
    }
}