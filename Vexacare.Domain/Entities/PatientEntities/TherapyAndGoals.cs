using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.SecondaryObjectives;
using Vexacare.Domain.Entities.Supliments;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class TherapyAndGoals
    {
        public int Id { get; set; }
        public bool? HasRecentAntibioticUse { get; set; }
        public string AntibioticName { get; set; }
        public DateTime? EndOfTherapyDate { get; set; }
        public ICollection<Supliment> Supplementation { get; set; }
        public string PrimaryHealthObjective { get; set; }
        public ICollection<SecondaryObjective> SecondaryObjectives { get; set; }

        // Navigation property
        public string PatientId { get; set; } // Foreign key to Patient/IdentityUser
        public Patient Patient { get; set; }

    }
}
