using AutoMapper;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels;
using Vexacare.Domain.Entities.ProductEntities;

namespace Vexacare.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBenefitRepository _benefitRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private const string ContainerName = "products";

        public ProductService(
            IProductRepository productRepository,
            IBenefitRepository benefitRepository,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _benefitRepository = benefitRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductListVM>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllWithBenefitsAsync();
            return _mapper.Map<IEnumerable<ProductListVM>>(products);
        }

        public async Task<AddProductVM> GetCreateProductModelAsync()
        {
            return new AddProductVM
            {
                AvailableBenefits = await _benefitRepository.GetAllAsync()
            };
        }

        public async Task CreateProductAsync(AddProductVM model)
        {
            string imageUrl = null;
            if (model.ProductImage != null)
            {
                imageUrl = await _fileStorageService.SaveFileAsync(model.ProductImage, ContainerName);
            }

            var product = _mapper.Map<Product>(model);
            product.ProductImages = imageUrl;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            if (model.SelectedBenefitIds != null)
            {
                product.ProductBenefits = model.SelectedBenefitIds.Select(id => new ProductBenefit
                {
                    BenefitId = id
                }).ToList();
            }
            await _productRepository.AddAsync(product);
        }

        public async Task<EditProductVM> GetEditProductModelAsync(int id)
        {
            var product = await _productRepository.GetByIdWithBenefitsAsync(id);
            if (product == null) return null;

            var model = _mapper.Map<EditProductVM>(product);
            model.AvailableBenefits = await _benefitRepository.GetAllAsync();
            model.SelectedBenefitIds = product.ProductBenefits.Select(pb => pb.BenefitId).ToList();

            return model;
        }

        public async Task UpdateProductAsync(EditProductVM model)
        {
            var product = await _productRepository.GetByIdWithBenefitsAsync(model.Id);
            if (product == null) throw new Exception("Product not found");

            // Handle image update
            if (model.ProductImage != null)
            {
                await _fileStorageService.DeleteFileAsync(product.ProductImages, ContainerName);
                product.ProductImages = await _fileStorageService.SaveFileAsync(model.ProductImage, ContainerName);
            }

            // Map only the properties that changed from the model to the existing product
            _mapper.Map(model, product);
            product.UpdatedAt = DateTime.UtcNow;
            //update product
            await _productRepository.UpdateAsync(product);
            // Update benefits
            await _productRepository.UpdateProductBenefitsAsync(product.Id, model.SelectedBenefitIds);          
            
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return;

            if (!string.IsNullOrEmpty(product.ProductImages))
            {
                await _fileStorageService.DeleteFileAsync(product.ProductImages, ContainerName);
            }

            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductDetailsVM> GetProductDetailsAsync(int id)
        {
            var product = await _productRepository.GetByIdWithBenefitsAsync(id);
            return _mapper.Map<ProductDetailsVM>(product);
        }

        public async Task CreateBenefitAsync(BenefitVM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var benefit = _mapper.Map<Benefit>(model);
            await _benefitRepository.AddAsync(benefit);
        }

        public Task DeleteBenefitAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}