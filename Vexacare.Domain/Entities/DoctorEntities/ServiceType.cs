using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int DoctorProfileId { get; set; }
        public ICollection<DoctorProfile> DoctorProfiles { get; set; }
    }
}
