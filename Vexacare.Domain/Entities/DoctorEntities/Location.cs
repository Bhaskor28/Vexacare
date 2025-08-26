namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int DoctorProfileId { get; set; }
        public ICollection<DoctorProfile> DoctorProfiles { get; set; }
    }
}
