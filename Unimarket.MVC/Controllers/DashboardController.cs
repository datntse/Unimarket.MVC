using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{
    public class DashboardController : Controller
	{
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<DashboardController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;
        public DashboardController(ILogger<DashboardController> logger, IHttpClientFactory httpClientFactory,
            IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _factory = httpClientFactory;
            _client = new HttpClient();
            _currentUserService = currentUserService;
            _client = _factory.CreateClient("ServerApi");
            _client.BaseAddress = new Uri(configuration["Cron:localhost"]);
        }
        public async Task<IActionResult> Index(DefaultSearch defaultSearch)
        {
            ResponseOrder cartItem = new ResponseOrder();
            var response = await _client.GetAsync(_client.BaseAddress + "Order/getall");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<ResponseOrder>(data);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            return View(cartItem);
        }

    }
}
