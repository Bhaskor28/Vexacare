using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.DoctorEntities;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Application.DoctorProfiles
{
    public class ProfileBasicVM
    {
        public int? Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        public int? ServiceTypeId { get; set; }
        public int? LocationId { get; set; }
        public int? CategoryId { get; set; }

        [MaxLength(500)]
        public string? AreaofExperties { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Gender { get; set; }

        [Column(TypeName = "text")]
        public string? About { get; set; }

        [Column(TypeName = "text")]
        public string? EducationDetails { get; set; }

        public IFormFile? DoctorImage { get; set; }  // Changed from string to IFormFile

        public string? ProfileImagePath { get; set; }
        public decimal? PatientCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public Category? Category { get; set; }
        public ServiceType? ServiceType { get; set; }
        public Location? Location { get; set; }
    }
}
