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
        // Physical Activity
        [Required(ErrorMessage = "Type of Activity is required")]
        [Display(Name = "Type of Activity")]
        [StringLength(100, ErrorMessage = "Type of Activity cannot exceed 100 characters")]
        public string ActivityType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sessions per week is required")]
        [Display(Name = "Sessions per week")]
        [Range(0, 20, ErrorMessage = "Must be between 0-20")]
        public int SessionsPerWeek { get; set; }

        [Required(ErrorMessage = "Average duration is required")]
        [Display(Name = "Average duration (minutes)")]
        [Range(0, 300, ErrorMessage = "Must be between 0-300 minutes")]
        public int AverageDurationMinutes { get; set; }

        // Sleep Quality
        [Required(ErrorMessage = "Average hours of sleep is required")]
        [Display(Name = "Average Hours of Sleep/Night")]
        [Range(0, 24, ErrorMessage = "Must be between 0-24 hours")]
        public double AverageHoursOfSleep { get; set; }

        [Required(ErrorMessage = "Sleep quality rating is required")]
        [Display(Name = "Sleep Quality Rating (1-10)")]
        [Range(1, 10, ErrorMessage = "Must be between 1-10")]
        public int SleepQualityRating { get; set; } = 5;

        [Required(ErrorMessage = "Specific sleep problems information is required")]
        [Display(Name = "Specific Sleep Problems")]
        [StringLength(500, ErrorMessage = "Specific sleep problems cannot exceed 500 characters")]
        public string SpecificProblems { get; set; } = string.Empty;

        // Stress and Habits
        [Required(ErrorMessage = "Stress level is required")]
        [Display(Name = "Stress Level (1-10)")]
        [Range(1, 10, ErrorMessage = "Must be between 1-10")]
        public int StressLevel { get; set; }

        [Required(ErrorMessage = "Please specify smoking habit")]
        [Display(Name = "Smoking Habit")]
        public bool IsSmoker { get; set; }

        [Display(Name = "Cigarettes per day (if smoker)")]
        [Range(0, 100, ErrorMessage = "Must be between 0-100")]
        public int CigarettesPerDay { get; set; } = 0;
    }
}