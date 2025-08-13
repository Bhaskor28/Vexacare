using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Domain.Entities.ProductEntities;
using Vexacare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Vexacare.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            ApplicationDbContext context,
            ILogger<ProductController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: All Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.ProductBenefits)
                .ThenInclude(pb => pb.Benefit)
                .AsNoTracking()
                .ToListAsync();

            return View(products);
        }

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
                    .ToListAsync()
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

            var model = new EditProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductImagePath = !string.IsNullOrEmpty(product.ProductImages)
                    ? $"/images/products/{product.ProductImages}"
                    : null,
                SelectedBenefitIds = product.ProductBenefits.Select(pb => pb.BenefitId).ToList(),
                AvailableBenefits = await _context.Benefits
                    .Select(b => new BenefitVM
                    {
                        Id = b.Id,
                        BenefitName = b.BenefitName
                    })
                    .ToListAsync()
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

            // Similar to Create but with update logic
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
                ProductImages = uniqueFileName,  // Store just the filename/path
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Update(product);
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
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}