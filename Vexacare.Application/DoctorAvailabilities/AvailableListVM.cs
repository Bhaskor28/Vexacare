using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.Availabilities;

namespace Vexacare.Application.DoctorAvailabilities
{
    public class AvailableListVM
    {
        public int Id { get; set; }
        public string DayName { get; set; }
        public bool IsAvailable { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
