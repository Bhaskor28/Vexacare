using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.Order;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        private readonly IMemoryCache _cache;

        public OrderService(ApplicationDbContext context, ICartService cartService, IMemoryCache cache)
        {
            _context = context;
            _cartService = cartService;
            _cache = cache;
        }

        public async Task<Order> CreateOrderAsync(CheckoutVM checkout, string userId)
        {
            // Get cart items
            var cart = await _cartService.GetCartAsync(userId);

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderNumber = await GenerateOrderNumberAsync(),
                FullName = checkout.FullName,
                Address = checkout.Address,
                Country = checkout.Country,
                City = checkout.City,
                ZipCode = checkout.ZipCode,
                PhoneNumber = checkout.PhoneNumber,
                OrderNotes = checkout.OrderNotes,
                Subtotal = checkout.Subtotal,
                Shipping = checkout.Shipping,
                Tax = checkout.Tax,
                Total = checkout.Total,
                Status = "Confirmed",
                OrderItems = cart.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };

            // Save to database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear cart after successful order
            await _cartService.ClearCartAsync(userId);

            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            return $"ORD-{timestamp}-{random}";
        }

        public async Task<decimal> CalculateShippingAsync(decimal subtotal)
        {
            // Free shipping for orders over $300
            if (subtotal >= 300)
                return 0;

            // Flat rate shipping
            return 20.99m;
        }

        public async Task<decimal> CalculateTaxAsync(decimal subtotal, string country)
        {
            // Different tax rates based on country
            return country.ToLower() switch
            {
                "usa" => subtotal * 0.08m, // 8% tax
                "canada" => subtotal * 0.13m, // 13% tax
                "uk" => subtotal * 0.20m, // 20% VAT
                _ => subtotal * 0.05m // Default 5% tax
            };
        }

        public async Task SaveCheckoutToCacheAsync(CheckoutVM checkout, string userId)
        {
            var cacheKey = $"Checkout_{userId}";
            _cache.Set(cacheKey, checkout, TimeSpan.FromHours(1)); // Store for 1 hour
        }

        public async Task<CheckoutVM> GetCheckoutFromCacheAsync(string userId)
        {
            var cacheKey = $"Checkout_{userId}";
            return _cache.Get<CheckoutVM>(cacheKey);
        }

        public async Task ClearCheckoutCacheAsync(string userId)
        {
            var cacheKey = $"Checkout_{userId}";
            _cache.Remove(cacheKey);
        }

        public async Task<bool> ProcessDummyPaymentAsync(CheckoutVM checkout, string userId)
        {
            try
            {
                // Simulate payment processing delay
                await Task.Delay(2000);

                // For demo purposes, always return success
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}