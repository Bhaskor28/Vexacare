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
        public int? FrequencyOfEvaluations { get; set; }
        public int? BristolScale { get; set; }
        public int? BloatingSeverity { get; set; }
        public int? IntestinalGas { get; set; }
        public int? AbdominalPain { get; set; }
        public int? DigestiveDifficulties { get; set; }
        public string DiagnosedIntolerances { get; set; }
        public string CertifiedAllergies { get; set; }
        public string TestsPerformed { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
