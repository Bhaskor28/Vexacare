using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class SymptomsInfo
    {
        public int Id { get; set; }

        // Intestinal Function
        [Display(Name = "Frequency of evacuations")]
        public int? FrequencyOfEvaluations { get; set; }

        [Display(Name = "Bristol Scale")]
        public int? BristolScale { get; set; }


        // Current Symptoms (0-10 scales)
        [Display(Name = "Bloating Severity")]
        public int? BloatingSeverity { get; set; }

        [Display(Name = "Intestinal Gas")]
        public int? IntestinalGas { get; set; }

        [Display(Name = "Abdominal Pain")]
        public int? AbdominalPain { get; set; }

        [Display(Name = "Digestive Difficulties")]
        public int? DigestiveDifficulties { get; set; }


        // Food Sensitivities
        [Display(Name = "Diagnosed Intolerances")]
        [StringLength(255)]
        public string DiagnosedIntolerances { get; set; }

        [Display(Name = "Certified Allergies")]
        [StringLength(255)]
        public string CertifiedAllergies { get; set; }

        [Display(Name = "Tests Performed")]
        [StringLength(255)]
        public string TestsPerformed { get; set; }


        [Required]
        public string PatientId { get; set; } // Foreign key to Patient

        // Navigation property
        public Patient Patient { get; set; }
    }
}
