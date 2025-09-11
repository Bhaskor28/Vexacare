using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Vexacare.Application.Bookings.ViewModels;
using Vexacare.Application.Interfaces;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Services.StripeServices;

namespace Vexacare.Web.Controllers
{
    public class BookingPaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StripeConfigService _stripeConfigService;
        private readonly IDoctorBookingService _doctorBookingServices;
        private readonly ILogger<BookingPaymentController> _logger;

        public BookingPaymentController(
            UserManager<ApplicationUser> userManager,
            StripeConfigService stripeConfigService,
            IDoctorBookingService doctorBookingServices,
            ILogger<BookingPaymentController> logger
            )
        {
            _userManager = userManager;
            _stripeConfigService = stripeConfigService;
            _doctorBookingServices = doctorBookingServices;
            _logger = logger;
        }

        public async Task<IActionResult> ValidatePayment()
        {
            try
            {
                // Get keys from database for validation
                var keys = await _stripeConfigService.GetStripeKeysForPaymentAsync();

                var service = new SessionService();

                // CORRECT: Use RequestOptions with ApiKey property
                var requestOptions = new RequestOptions { ApiKey = keys.SecretKey };

                // Now this will work - SessionGetOptions is optional first parameter
                Session session = service.Get(TempData["SessionId"].ToString(),
                    null, // SessionGetOptions (optional) - can be null
                    requestOptions); // RequestOptions with API key

                if (session.PaymentStatus == "paid")
                {
                    var userId = TempData["UserId"].ToString();
                    var partnerHub = await _doctorBookingServices.GetBookingFromCacheAsync(userId);
                    var BookingInfo = new DoctorBookingVM()
                    {
                        BookingNumber = $"BK{DateTime.Now.Ticks}",
                        BookingDate = DateTime.Now,
                        AppointmentDate = partnerHub.SelectedDate,
                        AppointmentTime = partnerHub.SelectedSlot[0], // Assuming single slot for simplicity
                        DoctorId = partnerHub.ProfileBasic.UserId,
                        PatientId = userId,
                        ConsultationFee = partnerHub.ProfileSession.PricePerConsultation,
                        NumberOfBookingSlots = partnerHub.SelectedSlot.Count,
                        TotalAmount = partnerHub.ProfileSession.PricePerConsultation * partnerHub.SelectedSlot.Count,
                        Status = Domain.Entities.Booking.BookingStatus.Confirmed,
                        PaymentStatus = Domain.Entities.Booking.PaymentStatus.Paid
                    };

                    // Create order
                    var booking = await _doctorBookingServices.CreateBookingAsync(BookingInfo, userId);
                    await _doctorBookingServices.ClearBookingFromCacheAsync(userId);

                    // Clear temp data
                    TempData.Remove("SessionId");
                    TempData.Remove("UserId");

                    // Redirect to order confirmation page
                    return RedirectToAction("Confirmed", "Consultations");
                }
                else
                {
                    return RedirectToAction("OrderFailed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating payment");
                return RedirectToAction("OrderFailed");
            }
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ProcessPayment()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new
                {
                    success = false,
                    message = "User is not authenticated."
                });
            }

            // Get checkout data from cache
            var partnerHub = await _doctorBookingServices.GetBookingFromCacheAsync(userId);
            if (partnerHub == null)
            {
                return Json(new { success = false, message = "Checkout data not found" });
            }

            // Get Stripe keys from database
            var keys = await _stripeConfigService.GetStripeKeysForPaymentAsync();

            // Pre-fill user info if available
            var user = await _userManager.GetUserAsync(User);

            var booking = new DoctorBookingVM();

            if (user != null)
            {
                booking.DoctorName = partnerHub.ProfileBasic.Name;
                booking.AppointmentDate = partnerHub.SelectedDate;
                booking.PatientName = user.FirstName + " " + user.LastName;
                booking.PatientEmail = user.Email;
                booking.NumberOfBookingSlots = partnerHub.SelectedSlot.Count;
                booking.TotalAmount = partnerHub.ProfileSession.PricePerConsultation * partnerHub.SelectedSlot.Count;
            }

            var domain = "http://localhost:5244/";
            //var domain = "http://vexacare.somee.com/";   // use this when published to somee.com

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"BookingPayment/ValidatePayment",
                CancelUrl = domain + "BookingPayment/BookingFailed",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = booking.PatientEmail
            };

            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalAmount * 100), // Amount in cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Consultation with Dr. {booking.DoctorName} on {booking.AppointmentDate:yyyy-MM-dd} at {booking.AppointmentTime}",
                        //Description = $"Booking Number: {booking.BookingNumber}"
                    }
                },
                Quantity = 1
            };

            options.LineItems.Add(sessionListItem);
            

            // Create the Stripe session using the secret key from database
            var service = new SessionService();
            var requestOptions = new RequestOptions { ApiKey = keys.SecretKey };

            Session session = service.Create(options, requestOptions); // CORRECT

            TempData["SessionId"] = session.Id;
            TempData["UserId"] = userId;

            return Json(new
            {
                success = true,
                redirectUrl = session.Url,
                sessionId = session.Id
            });
        }

        [Authorize(Roles = "Patient")]
        public IActionResult BookingFailed()
        {
            return View();
        }
    }
}
