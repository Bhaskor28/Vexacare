using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.Products.ViewModels
{
    public class BenefitVM
    {
        public int Id { get; set; }
        public string BenefitName { get; set; }
        public bool IsSelected { get; set; }
    }
}
