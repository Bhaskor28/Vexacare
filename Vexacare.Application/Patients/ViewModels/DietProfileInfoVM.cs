using System.ComponentModel.DataAnnotations;

namespace Vexacare.Application.Patients.ViewModels
{
    public class DietProfileInfoVM
    {
        // Diet Food
        [Required]
        [Display(Name = "Diet Food")]
        [StringLength(50)]
        public string DietFood { get; set; } // Omnivore/Vegetarian/Vegan/Other

        [Display(Name = "If Other, please specify your dietary preference")]
        [StringLength(100)]
        public string? DietTypeOther { get; set; }

        // Weekly Food Intake (servings)
        [Required]
        [Display(Name = "Vegetables")]
        public int Vegetables { get; set; } = 0;

        [Required]
        [Display(Name = "Fruits")]
        public int Fruits { get; set; } = 0;

        [Required]
        [Display(Name = "Whole Grains")]
        public int WholeGrains { get; set; } = 0;

        [Required]
        [Display(Name = "Animal Proteins")]
        public int AnimalProteins { get; set; } = 0;

        [Required]
        [Display(Name = "Plant Proteins")]
        public int PlantProteins { get; set; } = 0;

        [Required]
        [Display(Name = "Dairy Products")]
        public int DairyProducts { get; set; } = 0;

        [Required]
        [Display(Name = "Fermented Foods")]
        public int FermentedFoods { get; set; } = 0;

        // Hydration and Alcohol
        [Required]
        [Display(Name = "Water (liters/day)")]
        public decimal? Water { get; set; }

        [Required]
        [Display(Name = "Alcohol (units/week)")]
        public int? Alcohol { get; set; }

        // Typical Meal Times
        [Required]
        [Display(Name = "Breakfast")]
        public string BreakfastTime { get; set; }

        [Required]
        [Display(Name = "Lunch")]
        public string LunchTime { get; set; }

        [Required]
        [Display(Name = "Snacks")]
        public string SnacksTime { get; set; }

        [Required]
        [Display(Name = "Dinner")]
        public string DinnerTime { get; set; }
    }
}
