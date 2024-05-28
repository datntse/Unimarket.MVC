using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
