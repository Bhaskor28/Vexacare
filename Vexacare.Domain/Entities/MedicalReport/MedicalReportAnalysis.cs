using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.MedicalReport
{
    public class MedicalReportAnalysis
    {
        public PatientInformation PatientInformation { get; set; }
        public List<string> KeyFindings { get; set; }
        public List<string> Diagnoses { get; set; }
        public List<string> Medications { get; set; }
        public TestResults TestResults { get; set; }
        public List<string> Recommendations { get; set; }
        public string RiskAssessment { get; set; }
        public string Summary { get; set; }
    }

}
