using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Enums
{
    public enum ProductType
    {
        
        [Display(Name ="Test Kit")]
        TestKit=1,
        Supplement
    }
}
