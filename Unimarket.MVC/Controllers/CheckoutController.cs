using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Unimarket.MVC.Models.CreateModels;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{
    public class CheckoutController : Controller
    {
		private readonly IHttpClientFactory _factory;
		private readonly ILogger<CartController> _logger;
		private readonly HttpClient _client;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICurrentUserService _currentUserService;
		public CheckoutController(ILogger<CartController> logger, IHttpClientFactory httpClientFactory,
			IConfiguration configuration, ICurrentUserService currentUserService)
		{
			_factory = httpClientFactory;
			_client = new HttpClient();
			_currentUserService = currentUserService;
			_client = _factory.CreateClient("ServerApi");
			_client.BaseAddress = new Uri(configuration["Cron:localhost"]);
		}
		[HttpGet]
		public async Task<IActionResult> Index()
        {
            UserCartResponse cartItem = new UserCartResponse();
            var response = await _client.GetAsync(_client.BaseAddress + $"order/cart");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
				CheckOutVM CheckOutVM = new CheckOutVM
				{
					Cart = cartItem,
				};
				return View(CheckOutVM);
			}
			return RedirectToAction("OrderHistory", "Profile");
		}
		[HttpPost]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutCM model)
        {
            var userId = HttpContext.Session.GetString("UserId");
			CheckOutDTO checkOutDTO = new CheckOutDTO
			{
				UserId = userId,
				Address = model.Address,
				Note = model.Note,
				PaymentType = "Tiền mặt"
			};

            var response = await _client.PostAsync(_client.BaseAddress + $"Order/Checkout",
                 new StringContent(
                    JsonConvert.SerializeObject(checkOutDTO),
                    Encoding.UTF8,
                    "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
