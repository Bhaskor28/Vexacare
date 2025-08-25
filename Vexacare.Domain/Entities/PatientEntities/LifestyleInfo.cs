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

        // Physical Activity
        [StringLength(100)]
        public string ActivityType { get; set; }

        [Range(0, 20)]
        public int? SessionsPerWeek { get; set; }

        [Range(0, 300)]
        public int? AverageDurationMinutes { get; set; }

        // Sleep Quality
        [Range(0, 24)]
        public double? AverageHoursOfSleep { get; set; }

        [Range(1, 10)]
        public int? SleepQualityRating { get; set; }

        [StringLength(500)]
        public string SpecificProblems { get; set; }

        // Stress and Habits
        [Range(1, 10)]
        public int? StressLevel { get; set; }

        public bool? IsSmoker { get; set; }

        [Range(0, 100)]
        public int? CigarettesPerDay { get; set; }

        // Navigation
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
