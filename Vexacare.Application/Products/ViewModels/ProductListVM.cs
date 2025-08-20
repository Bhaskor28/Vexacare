using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Enums;

namespace Vexacare.Application.Products.ViewModels
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public string BreifDescription { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ProductImages { get; set; }
        public List<BenefitVM> ProductBenefits { get; set; }
    }
}
