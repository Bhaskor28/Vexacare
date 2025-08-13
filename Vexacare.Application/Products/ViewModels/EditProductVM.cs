using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Vexacare.Application.Products.ViewModels
{
    public class EditProductVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile ProductImage { get; set; }

        public string ProductImagePath { get; set; }

        [Display(Name = "Benefits")]
        public List<int> SelectedBenefitIds { get; set; } = new List<int>();

        public List<BenefitVM> AvailableBenefits { get; set; } = new List<BenefitVM>();
    }
}
