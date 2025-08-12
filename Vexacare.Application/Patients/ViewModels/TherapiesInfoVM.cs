using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Patients.ViewModels
{
    public class TherapiesInfoVM
    {
        public bool? UsedAntibioticsRecently { get; set; }
        public string? AntibioticName { get; set; }
        public DateTime? EndOfTherapyDate { get; set; }

        // Supplementation
        public bool UsesProbiotics { get; set; }
        public bool UsesPrebiotics { get; set; }
        public bool UsesMinerals { get; set; }
        public bool UsesVitamins { get; set; }
        public bool UsesOtherSupplements { get; set; }
        public string? OtherSupplementsDescription { get; set; }

        // Primary Health Objective
        public PrimaryHealthObjective? PrimaryObjective { get; set; }

        // Secondary Objectives
        public List<SecondaryObjective> SecondaryObjectives { get; set; } = new List<SecondaryObjective>();
    }

    public enum PrimaryHealthObjective
    {
        [Display(Name = "Preventive Wellbeing")]
        PreventiveWellbeing,

        [Display(Name = "Digestive Optimization")]
        DigestiveOptimization,

        [Display(Name = "Weight Management")]
        WeightManagement,

        [Display(Name = "Sports Performance")]
        SportsPerformance
    }

    public enum SecondaryObjective
    {
        [Display(Name = "Increased Energy")]
        IncreasedEnergy,

        [Display(Name = "Improved Sleep")]
        ImprovedSleep,

        [Display(Name = "Stress Reduction")]
        StressReduction,

        [Display(Name = "Hunger Control")]
        HungerControl,

        [Display(Name = "Muscle Recovery")]
        MuscleRecovery,

        [Display(Name = "Inflammation Reduction")]
        InflammationReduction,

        [Display(Name = "Hormone Balance")]
        HormoneBalance,

        [Display(Name = "Detoxification")]
        Detoxification,

        [Display(Name = "Skin Health")]
        SkinHealth,

        [Display(Name = "Other")]
        Other
    }
}
