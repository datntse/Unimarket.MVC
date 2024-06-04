using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unimarket.MVC.Models.CreateModels;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;
using System.Web;
using System.Reflection;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace Unimarket.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<UserController> _logger;
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public ProductController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory,
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
        public async Task<IActionResult> AddProduct()
        {
            var response = await _client.GetAsync(_client.BaseAddress + "category");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<CategoryVM>>(data);
                return View(result);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ItemCM itemCM, IFormFile mainImage, IFormFile subImage_1, IFormFile subImage_2, IFormFile subImage_3)
        {
            List<IFormFile> files = new List<IFormFile>();
            files.Add(mainImage);
            files.Add(subImage_1);
            files.Add(subImage_2);
            files.Add(subImage_3);

            String url = await FirebaseService.UploadImage(files);
            var listImage = url.Split("datnt").ToList();
            String mainIamge = listImage[0];
            listImage.RemoveAt(0);
            listImage.RemoveAt(3);

            List<String> category = new List<String>();
            category.Add(itemCM.CategoryId);
            ItemDTO itemDTO = new ItemDTO
            {
                CategoryId = category,
                ImageUrl = mainIamge,
                Quantity = itemCM.Quantity,
                Description = itemCM.Description,
                Name = itemCM.Name,
                Id = Guid.NewGuid(),
                Price = itemCM.Price,
                SubImageUrl = listImage.ToList()
            };

            var response = await _client.PostAsync(_client.BaseAddress + "Item",
                new StringContent(
                    JsonConvert.SerializeObject(itemDTO),
                    Encoding.UTF8,
                    "application/json"));
            if (response.IsSuccessStatusCode)
            {
                //var data = await response.Content.ReadAsStringAsync();au
                //var result = JsonConvert.DeserializeObject<ProductVM>(data);
                return View(null);
            }
            return View();
        }
    }
}
