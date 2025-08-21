using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class Availability
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public bool IsAvailable { get; set; } = true;
        //public int DoctorProfileId { get; set; }
        public ICollection<DoctorProfile> DoctorProfiles { get; set; }
    }
}
