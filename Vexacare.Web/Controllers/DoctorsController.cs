using Microsoft.AspNetCore.Mvc;

namespace Vexacare.Web.Controllers
{
    public class DoctorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
