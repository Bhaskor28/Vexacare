using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class GastrointestinalInfo
    {
        public int Id { get; set; }

        // GI Clinical History
        [StringLength(500)]
        public string PreviousGIProblems { get; set; }

        public DateTime? OnsetDateOfFirstSymptoms { get; set; }

        [StringLength(500)]
        public string TreatmentsPerformed { get; set; }

        // Family History
        [StringLength(500)]
        public string GIPathology { get; set; }

        [StringLength(500)]
        public string OtherRelevantMedicalConditions { get; set; }

        [StringLength(100)]
        public string DegreeOfRelationship { get; set; }

        // Surgical History
        [StringLength(200)]
        public string TypeOfSurgery { get; set; }

        public DateTime? DateOfSurgery { get; set; }

        [StringLength(500)]
        public string Outcome { get; set; }

        // Navigation property
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
