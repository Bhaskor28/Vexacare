using Vexacare.Application.Products.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vexacare.Application.Interfaces
{
    public interface IProductService
    {
        // Product Operations
        Task<IEnumerable<ProductListVM>> GetAllProductsAsync();
        Task<ProductDetailsVM> GetProductDetailsAsync(int id);
        Task CreateProductAsync(AddProductVM model);
        Task UpdateProductAsync(EditProductVM model);
        Task DeleteProductAsync(int id);

        // ViewModel Preparation
        Task<AddProductVM> GetCreateProductModelAsync();
        Task<EditProductVM> GetEditProductModelAsync(int id);

        // Benefit Operations
        Task CreateBenefitAsync(BenefitVM model);
        Task DeleteBenefitAsync(int id);
    }
}