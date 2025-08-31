using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.DoctorProfiles
{
    public class DayAvailabilityVM
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string DayName => DayOfWeek.ToString();

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0); // Default 9:00 AM

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; } = new TimeSpan(10, 0, 0); // Default 10:00 AM

        public bool IsAvailable { get; set; } = true;
    }
}
