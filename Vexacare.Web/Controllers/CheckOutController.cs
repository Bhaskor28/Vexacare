using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.PatientEntities;
using Stripe.Checkout;

namespace Vexacare.Web.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShopController> _logger;
        private readonly UserManager<Patient> _userManager;

        public CheckOutController(
            IProductService productService,
            ICartService cartService,
            IOrderService orderService,
            ILogger<ShopController> logger,
            UserManager<Patient> userManager)
        {
            _productService = productService;
            _cartService = cartService;
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
        }


        public async Task<IActionResult> ValidatePayment()
        {
            try {
                var service = new SessionService();
                Session session = service.Get(TempData["SessionId"].ToString());



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


        //[Authorize(Roles = "Patient")]
        //// GET: Checkout Page
        //public async Task<IActionResult> Checkout()
        //{
        //    var userId = _userManager.GetUserId(User);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
        //    }

        //    var cart = await _cartService.GetCartAsync(userId);
        //    if (!cart.Items.Any())
        //    {
        //        return RedirectToAction("Cart");
        //    }

        //    // Create checkout view model with cart data
        //    var checkout = new CheckoutVM
        //    {
        //        Subtotal = cart.Total,
        //        Shipping = await _orderService.CalculateShippingAsync(cart.Total),
        //        Tax = await _orderService.CalculateTaxAsync(cart.Total, "USA"),
        //        Total = cart.Total + await _orderService.CalculateShippingAsync(cart.Total) +
        //                await _orderService.CalculateTaxAsync(cart.Total, "USA"),
        //        CartItems = cart.Items
        //    };

        //    // Pre-fill user info if available
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user != null)
        //    {
        //        checkout.Email = user.Email;
        //        checkout.FullName = $"{user.FirstName} {user.LastName}";
        //    }

        //    var domain = "https://localhost:7236/";//your application domain

        //    var options = new SessionCreateOptions
        //    {
        //        SuccessUrl = domain + $"CheckOut/OrderConfirmation",
        //        CancelUrl = domain + "Shop/Checkout",
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //        CustomerEmail = checkout.Email
        //    };

        //    foreach (var item in checkout.CartItems)
        //    {
        //        var sessionListItem = new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                UnitAmount = (long)(item.Price * item.Quantity), // Convert to cents
        //                Currency = "usd",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.ProductName
        //                },
        //            },
        //            Quantity = item.Quantity
        //        };

        //        options.LineItems.Add(sessionListItem);
        //    }

        //    //setup stripe core
        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    TempData["Session"] = session.Id;

        //    Response.Headers.Add("Location", session.Url);  // Redirect to Stripe Checkout url
        //    return new StatusCodeResult(303); // Redirect to Stripe Checkout

        //    //return View(checkout);
        //}


        [Authorize(Roles = "Patient")]
        // GET: Checkout Page
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

            //payment

            // Pre-fill user info if available
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                checkout.Email = user.Email;
                checkout.FullName = $"{user.FirstName} {user.LastName}";
            }

            var domain = "https://localhost:7236/";//your application domain

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/ValidatePayment",
                CancelUrl = domain + "CheckOut/Orderfailed",
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
                        UnitAmount = (long)(item.Price * item.Quantity), // Convert to cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName
                        },
                    },
                    Quantity = item.Quantity
                };

                options.LineItems.Add(sessionListItem);
            }

            //setup stripe core
            var service = new SessionService();
            Session session = service.Create(options);

            TempData["SessionId"] = session.Id;
            TempData["UserId"] = userId;

            // Return the Stripe URL to the client instead of redirecting
            return Json(new
            {
                success = true,
                redirectUrl = session.Url,
                sessionId = session.Id
            });
        }

        // GET: Order Confirmation
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
