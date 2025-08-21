namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int DoctorProfileId { get; set; } which id will be there care it next time
        public ICollection<DoctorProfile> DoctorProfile { get; set; }
    }
}
