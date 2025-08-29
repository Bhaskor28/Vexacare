using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.Order;
using Vexacare.Domain.Enums;

namespace Vexacare.Application.Orders.OrderViewModel
{
    public class OrderVM
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? OrderNumber { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? OrderNotes { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public KitState? KitState { get; set; }
        public StateStatus? StateStatus { get; set; }
        public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    }
}


