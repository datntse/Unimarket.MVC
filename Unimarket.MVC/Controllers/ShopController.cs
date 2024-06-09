using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unimarket.MVC.Helpers;
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
            ResponseProductVM productList = new ResponseProductVM();

            // Construct the query string
            var queryParams = new List<string>
            {
                $"perPage={1}",
                $"currentPage={defaultSearch.currentPage}",
                $"sortBy={defaultSearch.sortBy}",
                $"isAscending={defaultSearch.isAscending}"
            };

            // Add category names to the query string
            if (defaultSearch.categoryNames != null && defaultSearch.categoryNames.Any())
            {
                queryParams.AddRange(defaultSearch.categoryNames.Select(c => $"categoryNames={Uri.EscapeDataString(c)}"));
            }

            // Add price range to the query string
            if (defaultSearch.MinPrice.HasValue)
            {
                queryParams.Add($"minPrice={defaultSearch.MinPrice.Value}");
            }
            if (defaultSearch.MaxPrice.HasValue)
            {
                queryParams.Add($"maxPrice={defaultSearch.MaxPrice.Value}");
            }

            var queryString = string.Join("&", queryParams);
            var response = await _client.GetAsync(_client.BaseAddress + $"Item?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                productList = JsonConvert.DeserializeObject<ResponseProductVM>(data);
            }

            return View(productList);
        }

    }
}
