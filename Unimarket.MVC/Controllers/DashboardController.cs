using Microsoft.AspNetCore.Mvc;

namespace Unimarket.MVC.Controllers
{
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	
	}
}
