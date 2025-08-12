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
        [Required]
        [Display(Name = "Frequency of evacuations")]
        public int? FrequencyOfEvaluations { get; set; }

        [Required]
        [Display(Name = "Bristol Scale")]
        public int? BristolScale { get; set; }


        // Current Symptoms (0-10 scales)
        [Required]
        [Display(Name = "Bloating Severity")]
        public int? BloatingSeverity { get; set; }

        [Required]
        [Display(Name = "Intestinal Gas")]
        public int? IntestinalGas { get; set; }

        [Required]
        [Display(Name = "Abdominal Pain")]
        public int? AbdominalPain { get; set; }

        [Required]
        [Display(Name = "Digestive Difficulties")]
        public int? DigestiveDifficulties { get; set; }


        // Food Sensitivities
        [Required]
        [Display(Name = "Diagnosed Intolerances")]
        [StringLength(255)]
        public string DiagnosedIntolerances { get; set; }

        [Required]
        [Display(Name = "Certified Allergies")]
        [StringLength(255)]
        public string CertifiedAllergies { get; set; }

        [Required]
        [Display(Name = "Tests Performed")]
        [StringLength(255)]
        public string TestsPerformed { get; set; }
    }
}
