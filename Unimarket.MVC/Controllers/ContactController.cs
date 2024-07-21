using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

        public IActionResult Welcome()
        {
            return View();
        }
    }
}
