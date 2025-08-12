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
    public class TherapiesInfo
    {
        public int Id { get; set; }
        public string PatientId { get; set; }

        // Antibiotic Use
        public bool? UsedAntibioticsRecently { get; set; }
        public string? AntibioticName { get; set; }
        public DateTime? EndOfTherapyDate { get; set; }

        // Supplementation
        public bool UsesProbiotics { get; set; }
        public bool UsesPrebiotics { get; set; }
        public bool UsesMinerals { get; set; }
        public bool UsesVitamins { get; set; }
        public bool UsesOtherSupplements { get; set; }
        public string? OtherSupplementsDescription { get; set; }

        // Health Objectives
        public int? PrimaryObjective { get; set; }
        public string SecondaryObjectives { get; set; }

        // Navigation
        public Patient Patient { get; set; }

    }
}
