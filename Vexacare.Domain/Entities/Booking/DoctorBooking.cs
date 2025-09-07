using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.Booking
{
    public class DoctorBooking
    {
        public int Id { get; set; }
        public string BookingNumber { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }

        // Doctor Information
        public int DoctorId { get; set; }
        
        // Patient Information
        public string PatientId { get; set; }
        
        // Payment Information
        public decimal ConsultationFee { get; set; }
        public int NumberOfBookingSlots { get; set; } = 1; 
        public decimal TotalAmount { get; set; }

        // Status
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled,
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }
}
