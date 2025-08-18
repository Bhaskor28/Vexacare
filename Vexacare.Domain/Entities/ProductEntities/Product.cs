using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Common;
using Vexacare.Domain.Enums;

namespace Vexacare.Domain.Entities.ProductEntities
{
    public class Product: BaseEntity
    {
        //public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public ProductType ProductType { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string? ProductImages { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<ProductBenefit> ProductBenefits { get; set; }

    }
}
