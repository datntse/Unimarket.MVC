using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
