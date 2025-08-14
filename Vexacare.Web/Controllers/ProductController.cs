using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Domain.Entities.ProductEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        #region Constructor
        public ProductController(
            ApplicationDbContext context,
            ILogger<ProductController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region DisplayAllProducts
        // GET: All Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.ProductBenefits)
                .ThenInclude(pb => pb.Benefit)
                .AsNoTracking()
                .ToListAsync();

            var model =  products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ProductImages = p.ProductImages,
                ProductBenefits = p.ProductBenefits.Select(pb => new BenefitVM
                {
                    Id = pb.Benefit.Id,
                    BenefitName = pb.Benefit.BenefitName
                }).ToList()
            }).ToList();
            return View(model);
        }

        #endregion

        #region CreateNewProduct
        // GET: Create New Product
        public async Task<IActionResult> Create()
        {
            var model = new AddProductVM
            {
                AvailableBenefits = await _context.Benefits
                    .Select(b => new BenefitVM
                    {
                        Id = b.Id,
                        BenefitName = b.BenefitName
                    })
                    .ToListAsync(),
            };

            return View(model);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableBenefits = await _context.Benefits
                    .Select(b => new BenefitVM
                    {
                        Id = b.Id,
                        BenefitName = b.BenefitName,
                        IsSelected = model.SelectedBenefitIds.Contains(b.Id)
                    })
                    .ToListAsync();

                return View(model);
            }

            try
            {
                // Handle file upload
                string uniqueFileName = null;
                if (model.ProductImage != null && model.ProductImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    await using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProductImage.CopyToAsync(fileStream);
                    }
                }

                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ProductType = model.ProductType,
                    ProductImages = uniqueFileName,  // Store just the filename/path
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (model.SelectedBenefitIds != null && model.SelectedBenefitIds.Any())
                {
                    var productBenefits = model.SelectedBenefitIds.Select(benefitId => new ProductBenefit
                    {
                        ProductId = product.Id,
                        BenefitId = benefitId
                    });

                    await _context.ProductBenefits.AddRangeAsync(productBenefits);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError("", "An error occurred while saving the product");

                model.AvailableBenefits = await _context.Benefits
                    .Select(b => new BenefitVM
                    {
                        Id = b.Id,
                        BenefitName = b.BenefitName,
                        IsSelected = model.SelectedBenefitIds.Contains(b.Id)
                    })
                    .ToListAsync();

                return View(model);
            }
        }

        #endregion

        #region EditProduct
        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductBenefits)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Get all available benefits first
            var availableBenefits = await _context.Benefits
                .Select(b => new BenefitVM
                {
                    Id = b.Id,
                    BenefitName = b.BenefitName
                })
                .ToListAsync();

            // Get selected benefit IDs
            var selectedBenefitIds = product.ProductBenefits.Select(pb => pb.BenefitId).ToList();

            // Set IsSelected for each benefit
            foreach (var benefit in availableBenefits)
            {
                benefit.IsSelected = selectedBenefitIds.Contains(benefit.Id);
            }

            var model = new EditProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ProductType = product.ProductType,
                Price = product.Price,
                ProductImagePath = !string.IsNullOrEmpty(product.ProductImages)
                    ? $"/images/products/{product.ProductImages}"
                    : null,
                SelectedBenefitIds = selectedBenefitIds,
                AvailableBenefits = availableBenefits
            };

            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableBenefits = await _context.Benefits
                    .Select(b => new BenefitVM
                    {
                        Id = b.Id,
                        BenefitName = b.BenefitName,
                        IsSelected = model.SelectedBenefitIds.Contains(b.Id)
                    })
                    .ToListAsync();
                return View(model);
            }

            var product = await _context.Products
                .Include(p => p.ProductBenefits)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (product == null)
            {
                return NotFound();
            }

            // Handle file upload
            string oldImagePath = null;
            if (model.ProductImage != null && model.ProductImage.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(product.ProductImages))
                {
                    oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", product.ProductImages);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Upload new image
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProductImage.CopyToAsync(fileStream);
                }

                product.ProductImages = uniqueFileName;
            }

            // Update product properties
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ProductType = model.ProductType;
            product.UpdatedAt = DateTime.UtcNow;

            // Update product benefits
            if (model.SelectedBenefitIds != null)
            {
                // Remove existing benefits not in the new selection
                var benefitsToRemove = product.ProductBenefits
                    .Where(pb => !model.SelectedBenefitIds.Contains(pb.BenefitId))
                    .ToList();

                foreach (var benefit in benefitsToRemove)
                {
                    product.ProductBenefits.Remove(benefit);
                }

                // Add new benefits not in the existing selection
                var existingBenefitIds = product.ProductBenefits.Select(pb => pb.BenefitId).ToList();
                var benefitsToAdd = model.SelectedBenefitIds
                    .Where(id => !existingBenefitIds.Contains(id))
                    .Select(benefitId => new ProductBenefit
                    {
                        ProductId = product.Id,
                        BenefitId = benefitId
                    });

                foreach (var benefit in benefitsToAdd)
                {
                    product.ProductBenefits.Add(benefit);
                }
            }
            else
            {
                // If no benefits selected, remove all existing ones
                product.ProductBenefits.Clear();
            }

            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                ModelState.AddModelError("", "An error occurred while updating the product");

                model.AvailableBenefits = await _context.Benefits
                    .Select(b => new BenefitVM
                    {
                        Id = b.Id,
                        BenefitName = b.BenefitName,
                        IsSelected = model.SelectedBenefitIds.Contains(b.Id)
                    })
                    .ToListAsync();

                return View(model);
            }
        }

        #endregion

        #region DeleteProduct
        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(product.ProductImages))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", product.ProductImages);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region ProductDetails
        // GET: Product/Details/5
        public async Task<IActionResult> ProductDetailsForAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductBenefits)
                .ThenInclude(pb => pb.Benefit)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductImagePath = !string.IsNullOrEmpty(product.ProductImages)
                    ? $"/images/products/{product.ProductImages}"
                    : null,
                Benefits = product.ProductBenefits.Select(pb => new BenefitVM
                {
                    Id = pb.Benefit.Id,
                    BenefitName = pb.Benefit.BenefitName
                }).ToList()
            };

            return View(viewModel);
        }

        #endregion
    }
}