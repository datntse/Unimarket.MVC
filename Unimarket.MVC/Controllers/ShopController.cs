using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models.CreateModels;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<ShopController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;
        public ShopController(ILogger<ShopController> logger, IHttpClientFactory httpClientFactory,
            IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _factory = httpClientFactory;
            _client = new HttpClient();
            _currentUserService = currentUserService;
            _client = _factory.CreateClient("ServerApi");
            _client.BaseAddress = new Uri(configuration["Cron:localhost"]);
        }
        [HttpGet]
        public async Task<IActionResult> Index(DefaultSearch defaultSearch)
        {
            ProductResponseApi productList = new ProductResponseApi();
            var response = await _client.GetAsync(_client.BaseAddress + $"product/all?page={0}&size={defaultSearch.perPage = 10}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                productList = JsonConvert.DeserializeObject<ProductResponseApi>(data);
            }
            return View(productList.Data);
        }
    }
}
