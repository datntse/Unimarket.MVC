using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IO;
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
            var userName = HttpContext.Session.GetString("User_FullName");
            UserCartResponse cartItem = new UserCartResponse();

            var response = await _client.GetAsync(_client.BaseAddress + $"order/cart");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
				CheckOutVM CheckOutVM = new CheckOutVM
				{
					User = new UserVM
					{
						FirstName = userName,
                    },
					Cart = cartItem,
				};
				return View(CheckOutVM);
			}
			return RedirectToAction("OrderHistory", "Profile");
		}
		[HttpPost]
        public async Task<IActionResult> ThanhToan()
        {
            var orderId = HttpContext.Session.GetString("OrderId");
            var response = await _client.PutAsync(_client.BaseAddress + $"payment/cod/{orderId}",
                 new StringContent(
                    JsonConvert.SerializeObject(null),
                    Encoding.UTF8,
                    "application/json"));
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Index", "Home");
            }
            return Ok();
           
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CheckoutAddress model)
        {
            var address = new
            {
                street = model.street,
                ward = model.Ward,
                district = model.District,
                province = model.Citi,
            };
            var response = await _client.PostAsync(_client.BaseAddress + $"address",
                 new StringContent(
                    JsonConvert.SerializeObject(address),
                    Encoding.UTF8,
                    "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var addressResponse = JsonConvert.DeserializeObject<AddressResponse>(data);

                response = await _client.PutAsync(_client.BaseAddress + $"order/address/{addressResponse.data}",
                 new StringContent(
                    JsonConvert.SerializeObject(null),
                    Encoding.UTF8,
                    "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var orderId = HttpContext.Session.GetString("OrderId");
                    response = await _client.PutAsync(_client.BaseAddress + $"payment/cod/{orderId}",
                         new StringContent(
                            JsonConvert.SerializeObject(null),
                            Encoding.UTF8,
                            "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.Remove("Cart");
                        return Ok();
                    }
                }
            }
            return Ok();
        }

    }
}
