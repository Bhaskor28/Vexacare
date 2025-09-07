using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.BlogManagement;
using Vexacare.Domain.Entities.DoctorEntities;

namespace Vexacare.Application.BlogPosts
{
    public class BlogPostVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [StringLength(500, ErrorMessage = "Excerpt cannot exceed 500 characters")]
        [Display(Name = "Excerpt")]
        
        public string Excerpt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Featured Image")]
        public IFormFile? FeaturedImage { get; set; }
        public string? FeaturedImagePath { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        //public ICollection<BlogCategory>? Categories { get; set; }

        [Display(Name = "Tags (comma separated)")]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Draft";
    }
}
