using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Vexacare.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ShopController> _logger;

        public ShopController(
            IProductService productService,
            ILogger<ShopController> logger)
        {
            _productService = productService;
            _logger = logger;
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

    }
}