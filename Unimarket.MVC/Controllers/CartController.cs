using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{

    public class CartController : Controller
    {
		private readonly IHttpClientFactory _factory;
		private readonly ILogger<CartController> _logger;
		private readonly HttpClient _client;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICurrentUserService _currentUserService;
		public CartController(ILogger<CartController> logger, IHttpClientFactory httpClientFactory,
			IConfiguration configuration, ICurrentUserService currentUserService)
		{
			_factory = httpClientFactory;
			_client = new HttpClient();
			_currentUserService = currentUserService;
			_client = _factory.CreateClient("ServerApi");
			_client.BaseAddress = new Uri(configuration["Cron:localhost"]);
		}
		public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> AddToCart(AddToCart item)
		{
			var userId = HttpContext.Session.GetString("UserId");

			if (userId == null)
			{
				return Json(new { success = false, message = "User is not logged in." });
			}

			item.UserId = userId;

			var jsonContent = JsonConvert.SerializeObject(item);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _client.PostAsync("Cart", content);

			if (response.IsSuccessStatusCode)
			{
				return Json(new { success = true, message = "Item added to cart successfully!" });
			}
			else
			{
				return Json(new { success = false, message = "Failed to add item to cart. Please try again." });
			}
		}
	}
}
