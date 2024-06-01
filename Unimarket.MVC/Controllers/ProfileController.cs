using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrderDetails()
        {
            return View();
        }
        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}
