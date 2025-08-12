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

        // Physical Activity Section
        [Display(Name = "Type of Activity")]
        [StringLength(100, ErrorMessage = "Activity type cannot exceed 100 characters.")]
        public string ActivityType { get; set; }

        [Display(Name = "Sessions per week")]
        [Range(0, 20, ErrorMessage = "Sessions must be between 0 and 20.")]
        public int? SessionsPerWeek { get; set; }

        [Display(Name = "Average duration (minutes)")]
        [Range(1, 300, ErrorMessage = "Duration must be between 1 and 300 minutes.")]
        public int? AverageDurationMinutes { get; set; }

        // Sleep Quality Section
        [Display(Name = "Average Hours of Sleep/Night")]
        [Range(0, 24, ErrorMessage = "Hours of sleep must be between 0 and 24.")]
        public double? AverageHoursOfSleep { get; set; }

        [Display(Name = "Sleep Quality Rating (1-10)")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int? SleepQualityRating { get; set; }

        [Display(Name = "Specific Sleep Problems")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string SleepProblems { get; set; }

        // Stress and Habits Section
        [Display(Name = "Stress Level (1-10)")]
        [Range(1, 10, ErrorMessage = "Stress level must be between 1 and 10.")]
        public int? StressLevel { get; set; }

        [Display(Name = "Eats Breakfast")]
        public bool? HasBreakfast { get; set; }

        [Display(Name = "Cigarettes per day (if smoker)")]
        [Range(0, 100, ErrorMessage = "Cigarettes must be between 0 and 100.")]
        public int? CigarettesPerDay { get; set; }
    }
}
