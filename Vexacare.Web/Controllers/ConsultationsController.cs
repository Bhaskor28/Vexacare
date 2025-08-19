using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vexacare.Web.Controllers
{
    public class ConsultationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        [Authorize(Roles = "Patient")]
        public IActionResult BookNow()
        {
            return View();
        }
        [Authorize(Roles = "Patient")]
        public IActionResult ConfirmPay()
        {
            return View();
        }
        [Authorize(Roles = "Patient")]
        public IActionResult Confirmed()
        {
            return View();
        }

    }
}
