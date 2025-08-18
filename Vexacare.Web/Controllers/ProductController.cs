using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Vexacare.Web.Controllers
{
    public class ProductController : Controller
    {
        #region Variable
        private readonly IProductService _productService;
        private readonly IBenefitRepository _benefitRepository;
        private readonly ILogger<ProductController> _logger;
        #endregion

        #region Ctor
        public ProductController(
            IProductService productService,
            IBenefitRepository benefitRepository,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _benefitRepository = benefitRepository;
            _logger = logger;
            
        }

        #endregion

        #region display all product
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        #endregion

        #region Create Product
        public async Task<IActionResult> Create()
        {
            var model = await _productService.GetCreateProductModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableBenefits = (await _productService.GetCreateProductModelAsync()).AvailableBenefits;
                return View(model);
            }

            try
            {
                await _productService.CreateProductAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError("", "An error occurred while saving the product");
                model.AvailableBenefits = (await _productService.GetCreateProductModelAsync()).AvailableBenefits;
                return View(model);
            }
        }

        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _productService.GetEditProductModelAsync(id);
            if (model == null) return NotFound();
            //model.AvailableBenefits = await _benefitRepository.GetAllAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableBenefits = (await _productService.GetEditProductModelAsync(model.Id))?.AvailableBenefits;
                return View(model);
            }

            try
            {
                await _productService.UpdateProductAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                ModelState.AddModelError("", "An error occurred while updating the product");
                model.AvailableBenefits = (await _productService.GetEditProductModelAsync(model.Id))?.AvailableBenefits;
                return View(model);
            }
        }

        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Product Details
        public async Task<IActionResult> ProductDetailsForAdmin(int id)
        {
            var viewModel = await _productService.GetProductDetailsAsync(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }
        #endregion
    }
}