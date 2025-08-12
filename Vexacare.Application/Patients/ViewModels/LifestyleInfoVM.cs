using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Patients.ViewModels
{
    public class LifestyleInfoVM
    {
        public int Id { get; set; }

        // Physical Activity
        [Display(Name = "Type of Activity")]
        public string ActivityType { get; set; }

        [Display(Name = "Sessions per week")]
        [Range(0, 20, ErrorMessage = "Must be between 0-20")]
        public int? SessionsPerWeek { get; set; }

        [Display(Name = "Average duration (minutes)")]
        [Range(0, 300, ErrorMessage = "Must be between 0-300 minutes")]
        public int? AverageDurationMinutes { get; set; }

        // Sleep Quality
        [Display(Name = "Average Hours of Sleep/Night")]
        [Range(0, 24, ErrorMessage = "Must be between 0-24 hours")]
        public double? AverageHoursOfSleep { get; set; }

        [Display(Name = "Sleep Quality Rating (1-10)")]
        [Range(1, 10, ErrorMessage = "Must be between 1-10")]
        public int? SleepQualityRating { get; set; }

        [Display(Name = "Specific Sleep Problems")]
        public string SpecificProblems { get; set; }

        // Stress and Habits
        [Display(Name = "Stress Level (1-10)")]
        [Range(1, 10, ErrorMessage = "Must be between 1-10")]
        public int? StressLevel { get; set; }

        [Display(Name = "Smoking Habit")]
        public bool? IsSmoker { get; set; }

        [Display(Name = "Cigarettes per day (if smoker)")]
        [Range(0, 100, ErrorMessage = "Must be between 0-100")]
        public int? CigarettesPerDay { get; set; }
    }
}
