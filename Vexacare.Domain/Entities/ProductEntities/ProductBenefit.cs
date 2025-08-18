using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Common;

namespace Vexacare.Domain.Entities.ProductEntities
{
    //map table of product and benefit
    public class ProductBenefit : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int BenefitId { get; set; }
        public Benefit Benefit { get; set; }

    }
}
