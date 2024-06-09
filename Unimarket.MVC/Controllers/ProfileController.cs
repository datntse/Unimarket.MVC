using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<ProfileController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;
        public ProfileController(ILogger<ProfileController> logger, IHttpClientFactory httpClientFactory,
            IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _factory = httpClientFactory;
            _client = new HttpClient();
            _currentUserService = currentUserService;
            _client = _factory.CreateClient("ServerApi");
            _client.BaseAddress = new Uri(configuration["Cron:localhost"]);
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var response = await _client.GetAsync(_client.BaseAddress + $"auth/profile/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserVM user = JsonConvert.DeserializeObject<UserVM>(data);
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> OrderHistory([FromQuery] DefaultSearch defaultSearch)
        {
            var userId = HttpContext.Session.GetString("UserId");
            ResponseOrder orders = new ResponseOrder();
            var response = await _client.GetAsync(_client.BaseAddress + $"Order/user?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<ResponseOrder>(data);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            return View(orders);
        }
        public async Task<IActionResult> OrderDetails([FromQuery] DefaultSearch defaultSearch, [FromQuery] string orderId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "User");
            }


            ResponseOrder orders = new ResponseOrder();
            var response = await _client.GetAsync(_client.BaseAddress + $"Order/user?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<ResponseOrder>(data);

                var order = orders.Data.FirstOrDefault(o => o.Id == Guid.Parse(orderId));
                if (order == null)
                {
                    return NotFound();
                }

                return View(order); 
            }
            else
            {
                return RedirectToAction("OrderHistory");
            }
        }

    }
}
