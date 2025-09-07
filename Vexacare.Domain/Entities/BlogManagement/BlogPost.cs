using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using Vexacare.Domain.Entities.DoctorEntities;

namespace Vexacare.Domain.Entities.BlogManagement
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string? FeaturedImagePath { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; } = "Draft";
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public IFormFile FeaturedImage { get; set; }
    }
}
    
