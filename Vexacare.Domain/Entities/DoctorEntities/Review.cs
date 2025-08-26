using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.DoctorEntities
{
    public class Review
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateOnly ReviewDate { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int DoctorProfileId { get; set; }
        public DoctorProfile DoctorProfile { get; set; } = null!;


    }
}
