using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
