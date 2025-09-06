using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vexacare.Domain.Enums;

namespace Vexacare.Application.Patients.ViewModels
{
    public class BasicInfoVM
    {
        [Display(Name = "Profile Picture")]
        [DataType(DataType.Upload)]
        public IFormFile? ProfilePicture { get; set; }

        //added by sazib
        public string? ProfilePictureUrl { get; set; } = "null";

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Please select a valid gender")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        [EnumDataType(typeof(Country), ErrorMessage = "Please select a valid country")]
        public Country Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "City can only contain letters, spaces, hyphens, and apostrophes")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postcode is required")]
        [Display(Name = "Postcode")]
        [StringLength(20, ErrorMessage = "Postcode cannot exceed 20 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Postcode can only contain letters, numbers, spaces, and hyphens")]
        public string Postcode { get; set; } = string.Empty;
    }
}