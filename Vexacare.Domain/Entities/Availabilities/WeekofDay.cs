using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.Availabilities
{
    public class WeekofDay
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AvailableDays> Availables { get; set; }


    }
}
