using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        #region Constructor
        public ShopController(
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

            var model = products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ProductType = p.ProductType,
                ProductImages = p.ProductImages,
                ProductBenefits = p.ProductBenefits.Select(pb => new BenefitVM
                {
                    Id = pb.Benefit.Id,
                    BenefitName = pb.Benefit.BenefitName
                }).ToList()
            }).ToList();
            return View(model);
        }
    }
    #endregion


}
