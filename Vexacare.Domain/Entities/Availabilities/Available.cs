using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.DoctorEntities;

namespace Vexacare.Domain.Entities.Availabilities
{
    public class Available
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ICollection<AvailableDays> WeekofDays { get; set; }

    }
}
