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

                return RedirectToAction("Index", "Home");
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
        // Add Edit, Details, Delete actions as needed...
    }
}