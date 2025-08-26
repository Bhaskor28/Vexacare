using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.DoctorEntities;

namespace Vexacare.Application.Locations
{
    public class LocationVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int DoctorProfileId { get; set; }
        public ICollection<DoctorProfile> DoctorProfiles { get; set; }
    }
}
