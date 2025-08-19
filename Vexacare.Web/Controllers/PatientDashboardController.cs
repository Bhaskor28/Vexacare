using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vexacare.Web.Controllers
{
    public class PatientDashboardController : Controller
    {
        [Authorize(Roles = "Patient")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
