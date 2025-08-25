using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Vexacare.Domain.Entities.PatientEntities;
using Vexacare.Application.Users.Doctors;

namespace Vexacare.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IDoctorService _doctorService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ShopController> _logger;

        public ShopController(
            IProductService productService,
            ICartService cartService,
            IDoctorService doctorService,
            ILogger<ShopController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _cartService = cartService;
            _doctorService = doctorService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();

                ViewBag.DoctorList = await _doctorService.GetAllDoctorAsync();
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




    }
}