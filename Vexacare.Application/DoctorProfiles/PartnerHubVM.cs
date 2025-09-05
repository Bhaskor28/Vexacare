using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vexacare.Application.DoctorProfiles
{
    public class PartnerHubVM
    {
        public ProfileBasicVM? ProfileBasic { get; set; }
        public DoctorSessionVM? ProfileSession { get; set; }
        public ChangePasswordVM? ChangePassword { get; set; }

        //add by Bhaskor 
        public DateTime? SelectedDate { get; set; }
        public string? SelectedDayName { get; set; }
        public List<TimeSpan>? SelectedSlot { get; set; } = new List<TimeSpan>();

        // You can add other properties for different tabs if needed
        //public object AnalysisReports { get; set; }
        //public object VideoConsultation { get; set; }
        //public object Notifications { get; set; }
    }
}
