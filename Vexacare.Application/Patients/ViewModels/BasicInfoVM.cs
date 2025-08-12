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
        public IFormFile? ProfilePicture { get; set; }

        //added by sazib
        public string? ProfilePictureUrl { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Postcode { get; set; }
    }
}
