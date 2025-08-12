using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Domain.Entities.Supliments
{
    public class Supliment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TherapyandGoalId { get; set; }
        public TherapyAndGoals TherapyAndGoals { get; set; }
    }
}
