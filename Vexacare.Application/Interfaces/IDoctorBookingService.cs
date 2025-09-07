using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Bookings.ViewModels;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.Booking;

namespace Vexacare.Application.Interfaces
{
    public interface IDoctorBookingService
    {
        // Booking methods
        Task<bool> CreateBookingAsync(DoctorBookingVM bookingVm, string userId);
        Task<PartnerHubVM> GetBookingFromCacheAsync(string userId);
        //Task<DoctorBookingVM> GetBookingByIdAsync(int bookingId);
        //Task<DoctorBookingVM> GetBookingByNumberAsync(string bookingNumber);
        //Task<List<DoctorBookingVM>> GetUserBookingsAsync(string userId);
        //Task<List<DoctorBookingVM>> GetDoctorBookingsAsync(int doctorId, DateTime? date = null);
        //Task<string> GenerateBookingNumberAsync();

        //// Admin methods
        //Task<List<DoctorBookingVM>> GetAllBookingsAsync(DateTime? startDate = null, DateTime? endDate = null);
        //Task<bool> UpdateBookingStatusAsync(int bookingId, BookingStatus status);
        //Task<bool> CancelBookingAsync(int bookingId);

        //// Availability methods
        //Task<List<TimeSpan>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date);
        //Task<bool> IsTimeSlotAvailableAsync(int doctorId, DateTime date, TimeSpan time);
    }
}
