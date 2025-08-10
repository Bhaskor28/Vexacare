using Microsoft.AspNetCore.Mvc;

namespace Vexacare.Web.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
