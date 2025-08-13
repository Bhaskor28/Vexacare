using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vexacare.Application.Products.ViewModels
{
    public class AddProductVM
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile? ProductImage { get; set; }  // Changed from string to IFormFile

        public string? ProductImagePath { get; set; }  // To store the path after upload

        [Display(Name = "Benefits")]
        public List<int> SelectedBenefitIds { get; set; } = new List<int>();

        public List<BenefitVM>? AvailableBenefits { get; set; } = new List<BenefitVM>();
    }
}