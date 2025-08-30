using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.Availabilities
{
    public class AvailableDays
    {
        public int Id { get; set; }
        public int AvailableId { get; set; }
        public Available Available { get; set; }
        public int WeekofDaysId { get; set; }
        public WeekofDay WeekofDay { get; set; }
    }
}
