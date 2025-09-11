using Microsoft.Extensions.Caching.Memory;
using Vexacare.Application.Bookings.ViewModels;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.Booking;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.DoctorBookingServices
{
    public class DoctorBookingService : IDoctorBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        public DoctorBookingService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<bool> CreateBookingAsync(DoctorBookingVM bookingVm, string userId)
        {
            try
            {
                var book = new DoctorBooking
                {
                    BookingNumber = bookingVm.BookingNumber,
                    BookingDate = bookingVm.BookingDate,
                    AppointmentDate = bookingVm.AppointmentDate,
                    AppointmentTime = bookingVm.AppointmentTime,

                    PatientId = bookingVm.PatientId,
                    DoctorId = bookingVm.DoctorId,
                    ConsultationFee = bookingVm.ConsultationFee ?? 0,
                    NumberOfBookingSlots = bookingVm.NumberOfBookingSlots ?? 1,
                    TotalAmount = bookingVm.TotalAmount ?? 0,
                    Status = bookingVm.Status,
                    PaymentStatus = PaymentStatus.Paid
                };
                _context.DoctorBookings.Add(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error creating booking: {ex.Message}");
                return false;
            }
        }

        public async Task<PartnerHubVM> GetBookingFromCacheAsync(string userId)
        {
            var cacheKey = $"Booking_{userId}";
            return _cache.Get<PartnerHubVM>(cacheKey);
        }
        public async Task ClearBookingFromCacheAsync(string userId)
        {
            var cacheKey = $"Booking_{userId}";
            _cache.Remove(cacheKey);
        }
    }
}
