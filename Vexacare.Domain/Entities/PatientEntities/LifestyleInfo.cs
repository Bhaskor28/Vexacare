using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class LifestyleInfo
    {
        public int Id { get; set; }
        public string ActivityType { get; set; }
        public int SessionsPerWeek { get; set; }
        public int AverageDurationMinutes { get; set; }
        public double AverageHoursOfSleep { get; set; }
        public int SleepQualityRating { get; set; }
        public string SpecificProblems { get; set; }
        public int StressLevel { get; set; }
        public bool IsSmoker { get; set; }
        public int CigarettesPerDay { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
