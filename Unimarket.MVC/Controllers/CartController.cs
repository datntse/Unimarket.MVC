using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models.CreateModels;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{

    public class CartController : Controller
    {
		private readonly IHttpClientFactory _factory;
		private readonly ILogger<CartController> _logger;
		private readonly HttpClient _client;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICurrentUserService _currentUserService;
		public CartController(ILogger<CartController> logger, IHttpClientFactory httpClientFactory,
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
			var userId = HttpContext.Session.GetString("UserId");
			ResponseCartVM cartItem = new ResponseCartVM();
			var response = await _client.GetAsync(_client.BaseAddress + $"Cart/get/usercart?userId={userId}");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				cartItem = JsonConvert.DeserializeObject<ResponseCartVM>(data);
			}
			else
			{
				return RedirectToAction("Login", "User");
			}
			return View(cartItem);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] string itemId)
        {
			var userId = HttpContext.Session.GetString("UserId");
			AddItemToCart item = new AddItemToCart();
			item.UserId = userId;
			item.ItemId = itemId;
			var response = await _client.PostAsync(_client.BaseAddress + "Cart", new StringContent(
					JsonConvert.SerializeObject(item),
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
		[HttpPost]
		public async Task<IActionResult> AddQuantityToCart([FromBody] AddToCarts item)
		{
			var userId = HttpContext.Session.GetString("UserId");
			UpdateItemQuantityDTO _item = new UpdateItemQuantityDTO();
			_item.UserId = userId;
			_item.ItemId = item.itemId;
			_item.Quantity = item.quantity;
			var response = await _client.PostAsync(_client.BaseAddress + "Cart/add-quantity", new StringContent(
					JsonConvert.SerializeObject(_item),
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

        [HttpPost] // Change to HttpPost to match the API controller
        public async Task<IActionResult> DeleteInCart(string itemId)
        {
            var userId = HttpContext.Session.GetString("UserId");

            var deleteItem = new AddItemToCart
            {
                UserId = userId,
                ItemId = itemId
            };

            var response = await _client.PostAsync(_client.BaseAddress + "Cart/delete-item-in-cart", new StringContent(
                    JsonConvert.SerializeObject(deleteItem),
                    Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Deleted successfully!";
                return Ok(response);
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to delete item from cart. Please try again." });
            }
        }
    }
}
