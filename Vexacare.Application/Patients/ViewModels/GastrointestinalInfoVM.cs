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
        public int Id { get; set; }

        // GI Clinical History
        [Display(Name = "Previous GI Problems")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string PreviousGIProblems { get; set; }

        [Display(Name = "Onset Date of First Symptoms")]
        [DataType(DataType.Date)]
        public DateTime? OnsetDateOfFirstSymptoms { get; set; }

        [Display(Name = "Treatments Performed")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string TreatmentsPerformed { get; set; }

        // Family History
        [Display(Name = "GI Pathologies")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string GIPathology { get; set; }

        [Display(Name = "Other Relevant Medical Conditions")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string OtherRelevantMedicalConditions { get; set; }

        [Display(Name = "Degree of Relationship")]
        [StringLength(100, ErrorMessage = "Cannot exceed 100 characters")]
        public string DegreeOfRelationship { get; set; }

        // Surgical History
        [Display(Name = "Type of Surgery")]
        [StringLength(200, ErrorMessage = "Cannot exceed 200 characters")]
        public string TypeOfSurgery { get; set; }

        [Display(Name = "Date of Surgery")]
        [DataType(DataType.Date)]
        public DateTime? DateOfSurgery { get; set; }

        [Display(Name = "Outcome")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string Outcome { get; set; }
    }
}
