using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class LifestyleInfo
    {
        public int Id { get; set; }

        // Physical Activity Section
        public string ActivityType { get; set; }
        public int? SessionsPerWeek { get; set; }
        public int? AverageDurationMinutes { get; set; }

        // Sleep Quality Section
        public double? AverageHoursOfSleep { get; set; }
        public int? SleepQualityRating { get; set; }
        public string SleepProblems { get; set; }

        // Stress and Habits Section
        public int? StressLevel { get; set; }
        public bool? HasBreakfast { get; set; }
        public int? CigarettesPerDay { get; set; }

        // Navigation property
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
