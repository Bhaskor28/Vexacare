using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Domain.Entities.ProductEntities;
using Vexacare.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vexacare.Domain.Enums;

namespace Vexacare.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllWithBenefitsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductBenefits)
                .ThenInclude(pb => pb.Benefit)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetByIdWithBenefitsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductBenefits)
                .ThenInclude(pb => pb.Benefit)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateProductBenefitsAsync(int productId, List<int> benefitIds)
        {
            // Get existing benefits WITHOUT tracking
            var existingBenefits = await _context.ProductBenefits
                .Where(pb => pb.ProductId == productId)
                .ToListAsync();

            // Determine benefits to remove
            var benefitsToRemove = existingBenefits
                .Where(eb => !benefitIds.Contains(eb.BenefitId))
                .ToList();

            if (benefitsToRemove.Any())
            {
                // Remove using stub entities
                _context.ProductBenefits.RemoveRange(benefitsToRemove);
            }

            // Determine benefits to add
            var existingBenefitIds = existingBenefits.Select(eb => eb.BenefitId).ToList();
            var benefitsToAdd = benefitIds
                .Where(bid => !existingBenefitIds.Contains(bid))
                .Select(benefitId => new ProductBenefit
                {
                    ProductId = productId,
                    BenefitId = benefitId
                })
                .ToList();

            if (benefitsToAdd.Any())
            {
                await _context.ProductBenefits.AddRangeAsync(benefitsToAdd);
            }
            await _context.SaveChangesAsync();

        }
        // Additional product-specific methods can be added here
        public async Task<IEnumerable<Product>> GetProductsByTypeAsync(ProductType productType)
        {
            return await _context.Products
                .Where(p => p.ProductType == productType)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}