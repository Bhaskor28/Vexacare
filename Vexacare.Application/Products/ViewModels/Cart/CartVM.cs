using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.ProductEntities.Cart;

namespace Vexacare.Application.Products.ViewModels.Cart
{
    public class CartVM
    {
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    }
}
