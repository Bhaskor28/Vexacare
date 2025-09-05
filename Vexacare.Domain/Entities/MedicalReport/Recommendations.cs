using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.MedicalReport
{
    public class Recommendations
    {
        public List<string> Diet { get; set; }
        public List<string> Supplements { get; set; }
        public List<string> GalenicForm { get; set; }
        public List<string> Lifestyle { get; set; }
    }
}
