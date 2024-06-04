using Microsoft.AspNetCore.Mvc;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{
	public class ProductDetailController : Controller
	{
		private readonly IHttpClientFactory _factory;
		private readonly ILogger<ProductDetailController> _logger;
		private readonly HttpClient _client;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICurrentUserService _currentUserService;
		public ProductDetailController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory,
			IConfiguration configuration, ICurrentUserService currentUserService)
		{
			_factory = httpClientFactory;
			_client = new HttpClient();
			_currentUserService = currentUserService;
			_client = _factory.CreateClient("ServerApi");
			_client.BaseAddress = new Uri(configuration["Cron:localhost"]);
		}
		[HttpGet("{id}")]
		public IActionResult Index(string id)
        {
            return View();
        }
    }
}
