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

        [HttpGet]
        public async Task<IActionResult> AddToCart(string itemId)
        {
			var response = await _client.PostAsync(_client.BaseAddress + "Cart", new StringContent(
					JsonConvert.SerializeObject(itemId),
					Encoding.UTF8,
					"application/json"));
			if (response.IsSuccessStatusCode)
			{
				ViewBag.SuccessMessage = "Registration successful!";


				return View();
			}
			else
			{
				ViewBag.ErrorMessage = "Please try again.";
				return RedirectToAction("Index","Home");
			}
		}
    }
}
