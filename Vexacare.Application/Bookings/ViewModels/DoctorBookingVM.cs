using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.Booking;

namespace Vexacare.Application.Bookings.ViewModels
{
    public class DoctorBookingVM
    {
        public int? Id { get; set; }
        public string? BookingNumber { get; set; }

        public DateTime? BookingDate { get; set; }
        public DateTime? AppointmentDate { get; set; }

        public TimeSpan? AppointmentTime { get; set; }

        // Doctor Information
        [Required]
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? DoctorEmail { get; set; }
        public string? DoctorPhoneNumber { get; set; }
        public string? DoctorSpecialization { get; set; }
        public string? DoctorProfileImageUrl { get; set; }

        // Patient Information
        [Required]
        public string PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? PatientEmail { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? PatientProfileImageUrl { get; set; }
        public string? PatientGender { get; set; }
        public string? PatientAddress { get; set; }

        // Payment Information
        public decimal? ConsultationFee { get; set; }
        public int? NumberOfBookingSlots { get; set; } = 1;
        public decimal? TotalAmount { get; set; }

        // Status
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;


    }
}
