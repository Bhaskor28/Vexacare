using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Infrastructure.Services.ProductServices;

namespace Vexacare.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShopController> _logger;
        private readonly UserManager<Patient> _userManager;

        public ShopController(
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

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return View("Error");
            }
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            try
            {
                var product = await _productService.GetProductDetailsAsync(id);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    // For anonymous users, you might want to use a session-based approach
                    // For this example, we'll require authentication
                    return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
                }

                await _cartService.AddToCartAsync(productId, quantity, userId);

                // Return the updated cart count
                var cartCount = await _cartService.GetCartItemCountAsync(userId);
                return Json(new { success = true, cartCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to cart");
                return Json(new { success = false, message = "Error adding product to cart" });
            }
        }

        public async Task<IActionResult> Cart()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }

            var cart = await _cartService.GetCartAsync(userId);

            return View(cart);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                await _cartService.RemoveFromCartAsync(productId, userId);

                // Return updated cart data
                var cart = await _cartService.GetCartAsync(userId);
                var cartCount = await _cartService.GetCartItemCountAsync(userId);

                return Json(new
                {
                    success = true,
                    cartCount,
                    cartTotal = cart.Total,
                    itemRemoved = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing product from cart");
                return Json(new { success = false, message = "Error removing product from cart" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                await _cartService.ClearCartAsync(userId);

                return Json(new
                {
                    success = true,
                    cartCount = 0,
                    cartTotal = 0.00m
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return Json(new { success = false, message = "Error clearing cart" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(int productId, int quantity)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                var cart = await _cartService.GetCartAsync(userId);
                var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

                if (item != null)
                {
                    if (quantity <= 0)
                    {
                        await _cartService.RemoveFromCartAsync(productId, userId);
                    }
                    else
                    {
                        // For this to work, you'll need to add an UpdateCart method to your service
                        item.Quantity = quantity;
                        // You'll need to implement a way to update the cart in your service
                    }
                }

                var updatedCart = await _cartService.GetCartAsync(userId);
                var cartCount = await _cartService.GetCartItemCountAsync(userId);

                return Json(new
                {
                    success = true,
                    cartCount,
                    cartTotal = updatedCart.Total
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return Json(new { success = false, message = "Error updating cart item" });
            }
        }




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

            return View(checkout);
        }

        // POST: Save Checkout Data to Cache
        [HttpPost]
        public async Task<IActionResult> SaveCheckoutToCache(CheckoutVM checkout)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            try
            {
                // Recalculate totals to ensure consistency
                var cart = await _cartService.GetCartAsync(userId);
                checkout.Subtotal = cart.Total;
                checkout.Shipping = await _orderService.CalculateShippingAsync(cart.Total);
                checkout.Tax = await _orderService.CalculateTaxAsync(cart.Total, checkout.Country);
                checkout.Total = checkout.Subtotal + checkout.Shipping + checkout.Tax;
                checkout.CartItems = cart.Items;

                // Save to cache
                await _orderService.SaveCheckoutToCacheAsync(checkout, userId);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving checkout data");
                return Json(new { success = false, message = "Error saving checkout data" });
            }
        }

        // POST: Process Dummy Payment
        [HttpPost]
        public async Task<IActionResult> ProcessDummyPayment()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            try
            {
                // Get checkout data from cache
                var checkout = await _orderService.GetCheckoutFromCacheAsync(userId);
                if (checkout == null)
                {
                    return Json(new { success = false, message = "Checkout data not found" });
                }

                // Process dummy payment
                var paymentSuccess = await _orderService.ProcessDummyPaymentAsync(checkout, userId);

                if (paymentSuccess)
                {
                    // Create order
                    var order = await _orderService.CreateOrderAsync(checkout, userId);

                    // Clear checkout cache
                    await _orderService.ClearCheckoutCacheAsync(userId);

                    return Json(new
                    {
                        success = true,
                        orderId = order.Id,
                        redirectUrl = Url.Action("OrderConfirmation", new { orderId = order.Id })
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Payment processing failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return Json(new { success = false, message = "Error processing payment" });
            }
        }

        // GET: Order Confirmation
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
    }
}