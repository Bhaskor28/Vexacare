using Vexacare.Application.Products.ViewModels;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Application.Interfaces
{
    public interface IBenefitRepository : IBaseRepository<Benefit>
    {
        Task<List<BenefitVM>> GetAllAsync();
    }
}
