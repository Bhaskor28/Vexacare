using System.ComponentModel.DataAnnotations;
using Vexacare.Domain.Enums;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class BasicInfo
    {
        public int Id { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Country Country { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
