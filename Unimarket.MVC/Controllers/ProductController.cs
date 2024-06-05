using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;
using System.Text;
using Unimarket.MVC.Models.CreateModels;
using System.Reflection.Metadata.Ecma335;
using Unimarket.MVC.Helpers;

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
        public async Task<IActionResult> Index()
        {
            ProductManageVM roductManageVM = new ProductManageVM();
            var responseProduct = await _client.GetAsync(_client.BaseAddress + "Item");
            var responseProductCategory = await _client.GetAsync(_client.BaseAddress + "category");
            if (responseProduct.IsSuccessStatusCode && responseProductCategory.IsSuccessStatusCode)
            {
                var dataProduct = await responseProduct.Content.ReadAsStringAsync();
                var dataCategory = await responseProductCategory.Content.ReadAsStringAsync();
                roductManageVM.Product = JsonConvert.DeserializeObject<ResponseProductVM>(dataProduct);
                roductManageVM.Categories = JsonConvert.DeserializeObject<List<CategoryVM>>(dataCategory);
            }
            return View(roductManageVM);
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

        [HttpGet]
        public async Task<IActionResult> UpdateProduct([FromQuery] string itemId)
        {
            var responseProduct = await _client.GetAsync(_client.BaseAddress + $"Item/get/{itemId}");
            var responseCategory = await _client.GetAsync(_client.BaseAddress + "category");

            if (responseProduct.IsSuccessStatusCode && responseCategory.IsSuccessStatusCode)
            {
                ProductUM productUM = new ProductUM();
                var data = await responseProduct.Content.ReadAsStringAsync();
                productUM.Product = JsonConvert.DeserializeObject<ProductVM>(data);
                var dateCate = await responseCategory.Content.ReadAsStringAsync();
                productUM.Categories = JsonConvert.DeserializeObject<List<CategoryVM>>(dateCate);
                return View(productUM);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductCM productCM)
        {
            List<String> category = new List<String>();
            category.Add(productCM.CategoryId);
            var responseProduct = await _client.PutAsync(_client.BaseAddress + "Item", new StringContent(
                  JsonConvert.SerializeObject(productCM),
                  Encoding.UTF8,
                  "application/json"));

            if (responseProduct.IsSuccessStatusCode )
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("UpdateProduct");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCM productCM, IFormFile mainImage, IFormFile subImage_1, IFormFile subImage_2, IFormFile subImage_3)
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
            category.Add(productCM.CategoryId);
            ProductDTO productDTO = new ProductDTO
            {
                CategoryId = category,
                ImageUrl = mainIamge,
                Quantity = productCM.Quantity,
                Description = productCM.Description,
                ProductDetail = productCM.ProductDetail,
                Name = productCM.Name,
                Id = Guid.NewGuid(),
                Price = productCM.Price,
                SubImageUrl = listImage.ToList()
            };

            var response = await _client.PostAsync(_client.BaseAddress + "Item",
                new StringContent(
                    JsonConvert.SerializeObject(productDTO),
                    Encoding.UTF8,
                    "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] String categoryName)
        {
            var response = await _client.PostAsync(_client.BaseAddress + "category", new StringContent(
                    JsonConvert.SerializeObject(categoryName),
                    Encoding.UTF8,
                    "application/json"));
            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Registration successful!";
                return Ok(response);
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to add item to cart. Please try again." });
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDTO category)
        {
            var response = await _client.PutAsync(
                _client.BaseAddress + "category",
                new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
            );

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to update category. Please try again." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromQuery] string categoryId)
        {
            string apiUrl = $"{_client.BaseAddress}category/delete-category?categoryId={categoryId}";

            // Send the DELETE request to the API
            var response = await _client.DeleteAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Return a success response if the deletion was successful
                return Ok(new { success = true });
            }
            else
            {
                // If deletion fails, return an error response
                return BadRequest(new { success = false, message = "Failed to delete category. Please try again." });
            }
        }
    }
}
