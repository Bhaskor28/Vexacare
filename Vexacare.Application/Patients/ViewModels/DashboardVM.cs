using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Enums;

namespace Vexacare.Application.Patients.ViewModels
{
    public class DashboardVM
    {
        public string Id { get; set; }
        public KitState? KitState { get; set; }
        public StateStatus? StateStatus { get; set; }

    }
}
