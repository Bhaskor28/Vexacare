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
        public string PreviousGIProblems { get; set; }
        public DateTime? OnsetDateOfFirstSymptoms { get; set; }
        public string TreatmentsPerformed { get; set; }
        public string GIPathology { get; set; }
        public string OtherRelevantMedicalConditions { get; set; }
        public string DegreeOfRelationship { get; set; }
        public string TypeOfSurgery { get; set; }
        public DateTime? DateOfSurgery { get; set; }
        public string Outcome { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
