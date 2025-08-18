using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Domain.Entities.ProductEntities;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Repositories
{
    public class BenefitRepository : BaseRepository<Benefit>, IBenefitRepository
    {
        private readonly ApplicationDbContext _context;

        public BenefitRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BenefitVM>> GetAllAsync()
        {
            return await _context.Benefits
                .Select(b => new BenefitVM
                {
                    Id = b.Id,
                    BenefitName = b.BenefitName
                })
                .ToListAsync();
        }
    }
}