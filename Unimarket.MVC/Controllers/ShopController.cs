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
            ResponseProductVM productList = new ResponseProductVM();

            var queryParams = new List<string>
            {
                $"perPage={9}", // Số sản phẩm trên mỗi trang, có thể điều chỉnh
                $"currentPage={defaultSearch.currentPage}",
                $"sortBy={defaultSearch.sortBy}",
                $"isAscending={defaultSearch.isAscending}"
            };

            if (defaultSearch.categoryNames != null && defaultSearch.categoryNames.Any())
            {
                queryParams.AddRange(defaultSearch.categoryNames.Select(c => $"categoryNames={Uri.EscapeDataString(c)}"));
                ViewData["categoryNames"] = defaultSearch.categoryNames;
            }

            if (defaultSearch.MinPrice.HasValue)
            {
                queryParams.Add($"minPrice={defaultSearch.MinPrice.Value}");
                ViewData["MinPrice"] = defaultSearch.MinPrice.Value;
            }
            if (defaultSearch.MaxPrice.HasValue)
            {
                queryParams.Add($"maxPrice={defaultSearch.MaxPrice.Value}");
                ViewData["MaxPrice"] = defaultSearch.MaxPrice.Value;
            }


            var queryString = string.Join("&", queryParams);
            List<CategoryVM> listCate = new List<CategoryVM>();
            var response = await _client.GetAsync(_client.BaseAddress + $"Item?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                productList = JsonConvert.DeserializeObject<ResponseProductVM>(data);
                var responseCategory = await _client.GetAsync(_client.BaseAddress + "category");
                var dateCate = await responseCategory.Content.ReadAsStringAsync();
                listCate = JsonConvert.DeserializeObject<List<CategoryVM>>(dateCate);
                ViewData["ListCate"] = listCate;

            }

            return View(productList);
        }

    }
}
