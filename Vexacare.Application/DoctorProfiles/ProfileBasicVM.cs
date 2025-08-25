using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.DoctorProfiles
{
    public class ProfileBasicVM
    {
        public string UserId { get; set; }
        public String Name { get; set; }
        public string ServiceType { get; set; }
        public string Location { get; set; }
        public string ServiceCategory { get; set; }
        public string AreaofExperties { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string  About { get; set; }
        public string EducationDetails { get; set; }
    }
}
