using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Entities.MedicalReport
{
    public class TestResults
    {
        public List<string> AbnormalResults { get; set; }
        public List<string> NormalResults { get; set; }
    }
}
