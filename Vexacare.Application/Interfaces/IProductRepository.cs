using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Application.Interfaces
{
    //Defines data access operations (abstraction)
    //in infrastructure, this will be implemented by ProductRepository
    public interface IProductRepository : IBaseRepository<Product>
    {
        // Add any product-specific repository method signatures here
        // Custom queries (e.g., search by name)
        //Task<List<Product>> SearchByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllWithBenefitsAsync();
        Task<Product> GetByIdWithBenefitsAsync(int id);
        Task UpdateProductBenefitsAsync(int productId, List<int> benefitIds);

    }
}

