using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Enums;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class BasicInfo
    {
        public int Id { get; set; }
        public string? ProfilePictureUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        public Gender Gender { get; set; }

        public Country Country { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string Postcode { get; set; }


        [Required]
        public string PatientId { get; set; } // Foreign key to Patient/IdentityUser

        // Navigation property
        public Patient Patient { get; set; }
    }
}
