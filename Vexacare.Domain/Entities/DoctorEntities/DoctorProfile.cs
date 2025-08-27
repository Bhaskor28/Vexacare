using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class DoctorProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AreaofExperties { get; set; }
        public string Gender { get; set; }
        public string About { get; set; }
        public string EducationDetails { get; set; }
        public string? ProfileImagePath { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        //public int ReviewId { get; set; }
        public decimal? PatientCount { get; set; } = 0;
        public ICollection<Review>? Reviews { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int ServiceTypeId { get; set; }
        public ServiceType? ServiceType { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public string UserId { get; set; }
        public int MyProperty { get; set; }
        public ApplicationUser User { get; set; }
        //public ICollection<SubCategory> SubCategories { get; set; }

    }
}
