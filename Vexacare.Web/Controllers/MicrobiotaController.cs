using Microsoft.AspNetCore.Mvc;

namespace Vexacare.Web.Controllers
{
    public class MicrobiotaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
