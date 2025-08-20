using Microsoft.Extensions.Caching.Memory;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels.Cart;
using Vexacare.Domain.Entities.ProductEntities.Cart;

namespace Vexacare.Infrastructure.Services.ProductServices
{
    public class CartService : ICartService
    {
        private readonly IMemoryCache _cache;
        private readonly IProductService _productService;

        public CartService(IMemoryCache cache, IProductService productService)
        {
            _cache = cache;
            _productService = productService;
        }


        public async Task AddToCartAsync(int productId, int quantity, string userId)
        {
            var cacheKey = GetCacheKey(userId);
            var cart = _cache.Get<Cart>(cacheKey) ?? new Cart { UserId = userId };

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var product = _productService.GetProductDetailsAsync(productId).Result;
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImagePath = product.ProductImagePath
                });
            }

            _cache.Set(cacheKey, cart, TimeSpan.FromDays(1));
        }

        public async Task RemoveFromCartAsync(int productId, string userId)
        {
            var cacheKey = GetCacheKey(userId);
            var cart = _cache.Get<Cart>(cacheKey);

            if (cart != null)
            {
                var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (itemToRemove != null)
                {
                    cart.Items.Remove(itemToRemove);
                    _cache.Set(cacheKey, cart, TimeSpan.FromDays(1));
                }
            }
        }

        public async Task<CartVM> GetCartAsync(string userId)
        {
            var cacheKey = GetCacheKey(userId);
            var cart = _cache.Get<Cart>(cacheKey);

            if (cart == null)
            {
                return new CartVM { UserId = userId };
            }

            // Convert Cart to CartVM
            return new CartVM
            {
                UserId = cart.UserId,
                Items = cart.Items.Select(item => new CartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImagePath = item.ImagePath
                }).ToList()
            };
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cacheKey = GetCacheKey(userId);
            var cart = _cache.Get<Cart>(cacheKey);
            return cart?.Items.Sum(i => i.Quantity) ?? 0;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cacheKey = GetCacheKey(userId);
            _cache.Remove(cacheKey);
        }

        private string GetCacheKey(string userId)
        {
            return $"Cart_{userId}";
        }
    }
}
