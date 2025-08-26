using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vexacare.Application.Interfaces;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Web.Components.Cart
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartCountViewComponent(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Content("0");
            }

            var cartCount = await _cartService.GetCartItemCountAsync(user.Id);
            return Content(cartCount.ToString());
        }
    }
}
