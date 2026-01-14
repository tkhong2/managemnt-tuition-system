using Microsoft.AspNetCore.Mvc;

namespace TuitionSys.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
