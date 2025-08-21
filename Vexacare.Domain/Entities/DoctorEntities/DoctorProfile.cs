namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class DoctorProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public int PatientsCount { get; set; }
        public string ConsultationType { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
        public string FeePeriod { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public string AreaofExperties { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        //public int ReviewId { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? SubCategory { get; set; }
        public int ServiceTypeId { get; set; }
        public ServiceType? ServiceType { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public int AvailabilityId { get; set; }
        public Availability Availability { get; set; }
    }
}
