using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.DoctorProfiles
{
    public class DoctorSessionVM
    {
        public int? DoctorProfileId { get; set; }

        // Consultation Settings
        public decimal? PricePerConsultation { get; set; }

        public int? SessionDuration { get; set; }

        // Availability Settings
        public List<DayAvailabilityVM> WeeklyAvailability { get; set; } = new List<DayAvailabilityVM>();
    }
}
