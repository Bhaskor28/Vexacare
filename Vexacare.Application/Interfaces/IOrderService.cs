using Vexacare.Application.Products.ViewModels.Checkout;
using Vexacare.Domain.Entities.Order;

namespace Vexacare.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CheckoutVM checkout, string userId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<string> GenerateOrderNumberAsync();
        Task<decimal> CalculateShippingAsync(decimal subtotal);
        Task<decimal> CalculateTaxAsync(decimal subtotal, string country);
        Task SaveCheckoutToCacheAsync(CheckoutVM checkout, string userId);
        Task<CheckoutVM> GetCheckoutFromCacheAsync(string userId);
        Task ClearCheckoutCacheAsync(string userId);
        Task<bool> ProcessDummyPaymentAsync(CheckoutVM checkout, string userId);
    }
}