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
        public string DietFood { get; set; }
        public string? DietTypeOther { get; set; }
        public int Vegetables { get; set; } = 0;
        public int Fruits { get; set; } = 0;
        public int WholeGrains { get; set; } = 0;
        public int AnimalProteins { get; set; } = 0;
        public int PlantProteins { get; set; } = 0;
        public int DairyProducts { get; set; } = 0;
        public int FermentedFoods { get; set; } = 0;
        public decimal? Water { get; set; }
        public int? Alcohol { get; set; }
        public string BreakfastTime { get; set; }
        public string LunchTime { get; set; }
        public string SnacksTime { get; set; }
        public string DinnerTime { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
