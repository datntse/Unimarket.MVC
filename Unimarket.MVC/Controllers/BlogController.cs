using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
