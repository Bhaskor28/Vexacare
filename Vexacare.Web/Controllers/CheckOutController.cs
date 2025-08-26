using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Services.StripeServices;

namespace Vexacare.Web.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShopController> _logger;
        private readonly UserManager<Patient> _userManager;
        private readonly StripeConfigService _stripeConfigService;

        public CheckOutController(
            IProductService productService,
            ICartService cartService,
            IOrderService orderService,
            ILogger<ShopController> logger,
            UserManager<Patient> userManager,
            StripeConfigService stripeConfigService)
        {
            _productService = productService;
            _cartService = cartService;
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
            _stripeConfigService = stripeConfigService;
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
                    var checkout = await _orderService.GetCheckoutFromCacheAsync(userId);

                    // Create order
                    var order = await _orderService.CreateOrderAsync(checkout, userId);
                    await _orderService.ClearCheckoutCacheAsync(userId);

                    // Clear temp data
                    TempData.Remove("SessionId");
                    TempData.Remove("UserId");

                    // Redirect to order confirmation page
                    return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
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
            var checkout = await _orderService.GetCheckoutFromCacheAsync(userId);
            if (checkout == null)
            {
                return Json(new { success = false, message = "Checkout data not found" });
            }

            // Get Stripe keys from database
            var keys = await _stripeConfigService.GetStripeKeysForPaymentAsync();

            // Pre-fill user info if available
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                checkout.Email = user.Email;
                checkout.FullName = $"{user.FirstName} {user.LastName}";
            }

            var domain = "http://localhost:5244/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/ValidatePayment",
                CancelUrl = domain + "CheckOut/OrderFailed",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = checkout.Email
            };

            foreach (var item in checkout.CartItems)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // Convert to cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                        },
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionListItem);
            }

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

        // ... rest of your controller methods remain the same
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null || order.UserId != userId)
            {
                return NotFound();
            }

            return View(order);
        }

        [Authorize(Roles = "Patient")]
        public IActionResult OrderFailed()
        {
            return View();
        }
    }
}
