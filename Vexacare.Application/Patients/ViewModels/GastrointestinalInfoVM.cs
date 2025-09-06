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
        // GI Clinical History
        [Required(ErrorMessage = "Previous GI Problems is required")]
        [Display(Name = "Previous GI Problems")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string PreviousGIProblems { get; set; } = string.Empty;

        [Required(ErrorMessage = "Onset Date of First Symptoms is required")]
        [Display(Name = "Onset Date of First Symptoms")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? OnsetDateOfFirstSymptoms { get; set; }

        [Required(ErrorMessage = "Treatments Performed is required")]
        [Display(Name = "Treatments Performed")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string TreatmentsPerformed { get; set; } = string.Empty;

        // Family History
        [Required(ErrorMessage = "GI Pathologies is required")]
        [Display(Name = "GI Pathologies")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string GIPathology { get; set; } = string.Empty;

        [Required(ErrorMessage = "Other Relevant Medical Conditions is required")]
        [Display(Name = "Other Relevant Medical Conditions")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string OtherRelevantMedicalConditions { get; set; } = string.Empty;

        [Required(ErrorMessage = "Degree of Relationship is required")]
        [Display(Name = "Degree of Relationship")]
        [StringLength(100, ErrorMessage = "Cannot exceed 100 characters")]
        public string DegreeOfRelationship { get; set; } = string.Empty;

        // Surgical History
        [Required(ErrorMessage = "Type of Surgery is required")]
        [Display(Name = "Type of Surgery")]
        [StringLength(200, ErrorMessage = "Cannot exceed 200 characters")]
        public string TypeOfSurgery { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Surgery is required")]
        [Display(Name = "Date of Surgery")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfSurgery { get; set; }

        [Required(ErrorMessage = "Outcome is required")]
        [Display(Name = "Outcome")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string Outcome { get; set; } = string.Empty;
    }
}