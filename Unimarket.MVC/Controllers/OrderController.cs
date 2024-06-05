using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
