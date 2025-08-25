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


        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                // Get orderId from TempData (or session, DB lookup, etc.)
                if (TempData["OrderId"] != null)
                {
                    int orderId = Convert.ToInt32(TempData["OrderId"]);
                    return RedirectToAction("OrderConfirmation", "Shop", new { orderId = orderId });
                }
            }

            return RedirectToAction("Index", "Shop");
        }




        [Authorize(Roles = "Patient")]
        // GET: Checkout Page
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }

            var cart = await _cartService.GetCartAsync(userId);
            if (!cart.Items.Any())
            {
                return RedirectToAction("Cart");
            }

            // Create checkout view model with cart data
            var checkout = new CheckoutVM
            {
                Subtotal = cart.Total,
                Shipping = await _orderService.CalculateShippingAsync(cart.Total),
                Tax = await _orderService.CalculateTaxAsync(cart.Total, "USA"),
                Total = cart.Total + await _orderService.CalculateShippingAsync(cart.Total) +
                        await _orderService.CalculateTaxAsync(cart.Total, "USA"),
                CartItems = cart.Items
            };

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
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                CancelUrl = domain + "Shop/Checkout",
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

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);  // Redirect to Stripe Checkout url
            return new StatusCodeResult(303); // Redirect to Stripe Checkout

            //return View(checkout);
        }

        
    }
}
