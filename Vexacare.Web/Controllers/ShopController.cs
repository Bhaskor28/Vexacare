using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Application.Users.Doctors;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Infrastructure.Services.ProductServices;

namespace Vexacare.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShopController> _logger;

        public ShopController(
            IProductService productService,
            ICartService cartService,
            IDoctorService doctorService,
            IDoctorProfileService doctorProfileService,
            IOrderService orderService,
            ILogger<ShopController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _cartService = cartService;
            _doctorProfileService = doctorProfileService;
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
        }
        #region index
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                ViewBag.DoctorList = await _doctorProfileService.GetAllDoctorProfiles();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return View("Error");
            }
        }
        #endregion

        #region product detail
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

        #endregion


        #region Add to cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "You are not a valid user!! Please login first." });
                }

                await _cartService.AddToCartAsync(productId, quantity, userId);

                // Return the updated cart count
                var cartCount = await _cartService.GetCartItemCountAsync(userId);
                return Json(new { success = true, cartCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to cart");
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region Cart

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
        #endregion

        #region Remove from cart
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

        #endregion

        #region Clear Cart
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

        #endregion

        #region update cart item 
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

        #endregion

        #region checkout
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
        #endregion

        #region save checkout to cache
        // POST: Save Checkout Data to Cache
        [Authorize(Roles = "Patient")]
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
        #endregion


        #region Order Confirmation

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

        #endregion
    }
}