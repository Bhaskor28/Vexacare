using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Vexacare.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    ViewData["Layout"] = "_AdminLayout";
                }
                else if (User.IsInRole("Doctor"))
                {
                    ViewData["Layout"] = "_DoctorLayout";
                }
                else if (User.IsInRole("Patient"))
                {
                    ViewData["Layout"] = "_PatientLayout";
                }
            }

            base.OnActionExecuted(context);
        }
    }
}