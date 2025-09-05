using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.MedicalReport
{
    public class MedicalReportAnalysis
    {
        public PatientInformation PatientInformation { get; set; } = new PatientInformation();
        public List<string> KeyFindings { get; set; } = new List<string>();
        public List<string> Diagnoses { get; set; } = new List<string>();
        public List<string> Medications { get; set; } = new List<string>();
        public TestResults TestResults { get; set; } = new TestResults();
        public RecommendationObject Recommendations { get; set; } = new RecommendationObject(); // Changed from List<string>
        public string RiskAssessment { get; set; }
        public string Summary { get; set; }
    }

    // Add these new classes for the recommendation structure
    public class RecommendationObject
    {
        public List<DietRecommendation> Diet { get; set; } = new List<DietRecommendation>();
        public List<SupplementRecommendation> Supplements { get; set; } = new List<SupplementRecommendation>();
        public List<GalenicFormRecommendation> GalenicForm { get; set; } = new List<GalenicFormRecommendation>();
        public List<LifestyleRecommendation> Lifestyle { get; set; } = new List<LifestyleRecommendation>();
    }

    public class DietRecommendation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Impact { get; set; }
        public string Duration { get; set; }
        public string KeyPoints { get; set; }
        public List<string> MainBenefits { get; set; } = new List<string>();
        public string HowToUse { get; set; }
        public string ApplicationFrequency { get; set; }
        public string Caution { get; set; }
        public string Goal { get; set; }
    }

    public class SupplementRecommendation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Purpose { get; set; }
        public List<string> Benefits { get; set; } = new List<string>();
        public string Instructions { get; set; }
        public string Precautions { get; set; }
        public string Interactions { get; set; }
    }

    public class GalenicFormRecommendation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FormType { get; set; }
        public List<string> Advantages { get; set; } = new List<string>();
        public string UsageInstructions { get; set; }
        public string Duration { get; set; }
        public string Compatibility { get; set; }
    }

    public class LifestyleRecommendation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public List<string> Benefits { get; set; } = new List<string>();
        public string ImplementationSteps { get; set; }
        public string TimeRequired { get; set; }
        public string Precautions { get; set; }
    }
}
