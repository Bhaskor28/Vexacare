using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.MedicalReport
{
    public class MedicalReportAnalysisResult
    {
        public bool Success { get; set; }
        public string FileName { get; set; }
        public int ExtractedTextLength { get; set; }
        public MedicalReportAnalysis Analysis { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
