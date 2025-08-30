using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Common;

namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class Availability : BaseEntity
    {
        
        [Required]
        public int? DoctorProfileId { get; set; }

        [ForeignKey("DoctorProfileId")]
        public DoctorProfile? DoctorProfile { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;

        //added by sazib
        // Add these properties for better time slot management
        public int? SlotDuration { get; set; } = 60; // Duration in minutes
        
    }
}
