using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Patients.ViewModels
{
    public class TherapiesInfoVM
    {
        public int Id { get; set; }

        [Display(Name = "Has Recent Antibiotic Use?")]
        public bool? HasRecentAntibioticUse { get; set; }

        [Display(Name = "Antibiotic Name")]
        public string AntibioticName { get; set; }

        [Display(Name = "End of Therapy Date")]
        [DataType(DataType.Date)]
        public DateTime? EndOfTherapyDate { get; set; }

        [Display(Name = "Supplementation")]
        public List<int> SupplementationIds { get; set; } = new List<int>();

        [Display(Name = "Primary Health Objective")]
        public string PrimaryHealthObjective { get; set; }

        [Display(Name = "Secondary Objectives")]
        public List<int> SecondaryObjectiveIds { get; set; } = new List<int>();

        // For selecting in dropdowns/multi-selects
        public List<SupplementViewModel> AvailableSupplements { get; set; } = new List<SupplementViewModel>();
        public List<SecondaryObjectiveViewModel> AvailableSecondaryObjectives { get; set; } = new List<SecondaryObjectiveViewModel>();

        // Patient info
        public string PatientId { get; set; }
        public class SupplementViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class SecondaryObjectiveViewModel
        {
            public int Id { get; set; }
            public string Objective { get; set; }
        }
    }
}
