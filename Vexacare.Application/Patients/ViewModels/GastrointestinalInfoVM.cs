using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Patients.ViewModels
{
    public class GastrointestinalInfoVM
    {
        public string Id { get; set; }

        [Display(Name = "Previous GI Problems")]
        public string PreviousGIProblems { get; set; }

        [Display(Name = "Onset Date of First Symptoms")]
        [DataType(DataType.Date)]
        public DateTime? OnsetDateOfFirstSymptoms { get; set; }

        [Display(Name = "Treatments Performed")]
        public string TreatmentsPerformed { get; set; }

        [Display(Name = "GI Pathology")]
        public string GIPathology { get; set; }

        [Display(Name = "Degree of Relationship")]
        public string DegreeOfRelationship { get; set; }

        [Display(Name = "Other Relevant Medical Conditions")]
        public string OtherRelevantMedicalConditions { get; set; }

        [Display(Name = "Type of Surgery")]
        public string TypeOfSurgery { get; set; }

        [Display(Name = "Date of Surgery")]
        [DataType(DataType.Date)]
        public DateTime? DateOfSurgery { get; set; }

        [Display(Name = "Outcome")]
        public string Outcome { get; set; }
    }
}
