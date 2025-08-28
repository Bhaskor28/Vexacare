using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Domain.Enums
{
    public enum KitState
    {
        None = 0,
        KitOrdered = 1,
        SampleReceived = 2,
        InAnalysis = 3,
        AIProcessing = 4,
        MedicalValidation = 5,
        ResultsAvailable = 6
    }
}
