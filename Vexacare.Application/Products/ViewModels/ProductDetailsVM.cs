using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vexacare.Domain.Entities.ProductEntities;
using Vexacare.Domain.Enums;

namespace Vexacare.Application.Products.ViewModels
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Product Type")]
        public ProductType? ProductType { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile? ProductImage { get; set; }

        public string? ProductImagePath { get; set; }

        [Display(Name = "Benefits")]
        public List<BenefitVM>? ProductBenefits { get; set; }
    }
}
