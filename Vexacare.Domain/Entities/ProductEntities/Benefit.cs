using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Common;

namespace Vexacare.Domain.Entities.ProductEntities
{
    public class Benefit : BaseEntity
    {
        public string BenefitName { get; set; }
        public virtual ICollection<ProductBenefit> ProductBenefits { get; set; }
    }
}
