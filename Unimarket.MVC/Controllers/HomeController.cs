using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<UserController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory,
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

            ResponseProductVM productList = new ResponseProductVM();
            var response = await _client.GetAsync(_client.BaseAddress + $"Item?perPage={defaultSearch.perPage = 8}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                productList = JsonConvert.DeserializeObject<ResponseProductVM>(data);
            }

            return View(productList);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
