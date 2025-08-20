using Vexacare.Application.Products.ViewModels.Cart;

namespace Vexacare.Application.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(int productId, int quantity, string userId);
        Task RemoveFromCartAsync(int productId, string userId);
        Task<CartVM> GetCartAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task ClearCartAsync(string userId);
    }
}
