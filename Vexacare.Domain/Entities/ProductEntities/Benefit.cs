using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.ProductEntities
{
    public class Benefit
    {
        public int Id { get; set; }
        public string BenefitName { get; set; }
        public virtual ICollection<ProductBenefit> ProductBenefits { get; set; }
    }
}
