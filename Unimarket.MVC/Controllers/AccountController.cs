using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
