using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.PatientEntities
{
    public class DietProfileInfo
    {
        public int Id { get; set; }

        // Diet Food
        [Display(Name = "Diet Food")]
        [StringLength(50)]
        public string DietFood { get; set; } // Omnivore/Vegetarian/Vegan/Other

        [Display(Name = "If Other, please specify your dietary preference")]
        [StringLength(100)]
        public string? DietTypeOther { get; set; }

        // Weekly Food Intake (servings)
        [Display(Name = "Vegetables")]
        public int Vegetables { get; set; } = 0;

        [Display(Name = "Fruits")]
        public int Fruits { get; set; } = 0;

        [Display(Name = "Whole Grains")]
        public int WholeGrains { get; set; } = 0;

        [Display(Name = "Animal Proteins")]
        public int AnimalProteins { get; set; } = 0;

        [Display(Name = "Plant Proteins")]
        public int PlantProteins { get; set; } = 0;

        [Display(Name = "Dairy Products")]
        public int DairyProducts { get; set; } = 0;

        [Display(Name = "Fermented Foods")]
        public int FermentedFoods { get; set; } = 0;

        // Hydration and Alcohol
        [Display(Name = "Water (liters/day)")]
        public decimal? Water { get; set; }

        [Display(Name = "Alcohol (units/week)")]
        public int? Alcohol { get; set; }

        // Typical Meal Times
        [Display(Name = "Breakfast")]
        public string BreakfastTime { get; set; }

        [Display(Name = "Lunch")]
        public string LunchTime { get; set; }

        [Display(Name = "Snacks")]
        public string SnacksTime { get; set; }

        [Display(Name = "Dinner")]
        public string DinnerTime { get; set; }


        [Required]
        public string PatientId { get; set; } // Foreign key to Patient

        // Navigation property
        public Patient Patient { get; set; }
    }
}
