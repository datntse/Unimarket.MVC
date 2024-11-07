using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models.CreateModels;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
            UserCartResponse cartItem = new UserCartResponse();
            var response = await _client.GetAsync(_client.BaseAddress + $"order/cart");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
            }
            if (cartItem.Data == null)
            {
                cartItem.Data = new CartDTO();
                cartItem.Data.orderDetails = new List<OrderDetails>();
            }
            return View(cartItem.Data);
        }
        public async Task<string> RenderViewAsync<TModel>(string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.ActionDescriptor.ActionName;
            }

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return writer.GetStringBuilder().ToString();
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateCart model)
        {
            UserCartResponse cartItem = new UserCartResponse();
            var response = await _client.GetAsync(_client.BaseAddress + $"order/cart");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
            }
            var quantity = cartItem.Data.orderDetails.Where(_ => _.id.Equals(model.ItemId)).Select(_ => _.quantity).FirstOrDefault();
            if (model.Status.Equals("up"))
            {
                quantity += 1;
                var quantityObject = new
                {
                    quantity,
                };
                response = await _client.PutAsync(_client.BaseAddress + $"details/{model.ItemId}", new StringContent(
                     JsonConvert.SerializeObject(quantityObject),
                     Encoding.UTF8,
                     "application/json"));

            }
            else
            {
                quantity -= 1;
                if (quantity == 0)
                {
                    response = await _client.DeleteAsync(_client.BaseAddress + $"details/{model.ItemId}");
                }
                else
                {
                    var quantityObject = new
                    {
                        quantity,
                    };
                    response = await _client.PutAsync(_client.BaseAddress + $"details/{model.ItemId}", new StringContent(
                          JsonConvert.SerializeObject(quantityObject),
                          Encoding.UTF8,
                          "application/json"));
                }

            }
            if (response.IsSuccessStatusCode)
            {

                response = await _client.GetAsync(_client.BaseAddress + $"order/cart");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
                }
                string htmlContent = await RenderViewAsync("UpdateCart", cartItem.Data, true);
                return Json(htmlContent);
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to add item to cart. Please try again." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] string itemId)
        {
            var user = HttpContext.Session.GetString("User_FullName");
            if (user == null)
            {
                return Json(new { success = false, requiresLogin = true });
            }

            var cartQuantity = new
            {
                quantity = 1
            };

            var response = await _client.PostAsync(_client.BaseAddress + $"details/{itemId}", new StringContent(
                    JsonConvert.SerializeObject(cartQuantity),
                    Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseData = await _client.GetAsync(_client.BaseAddress + $"order/cart");
                if (responseData.IsSuccessStatusCode)
                {
                    var data = await responseData.Content.ReadAsStringAsync();
                    var cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
                    HttpContext.Session.SetInt32("Cart", cartItem.Data.orderDetails.Count());
                }
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to add item to cart. Please try again." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddQuantityToCart([FromBody] AddToCarts item)
        {
            var userId = HttpContext.Session.GetString("User_FullName");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }
            UserCartResponse cartItem = new UserCartResponse();
            var response = await _client.GetAsync(_client.BaseAddress + $"order/cart");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
            }
            var quantity = cartItem.Data.orderDetails.Where(_ => _.id.Equals(item.itemId)).Select(_ => _.quantity).FirstOrDefault();

            if (quantity > 0)
            {
                quantity += item.quantity;
                var quantityObject = new
                {
                    quantity,
                };
                response = await _client.PutAsync(_client.BaseAddress + $"details/{item.itemId}", new StringContent(
                     JsonConvert.SerializeObject(quantityObject),
                     Encoding.UTF8,
                     "application/json"));
            }

            var cartQuantity = new
            {
                quantity = item.quantity,
            };

            response = await _client.PostAsync(_client.BaseAddress + $"details/{item.itemId}", new StringContent(
                   JsonConvert.SerializeObject(cartQuantity),
                   Encoding.UTF8,
                   "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseData = await _client.GetAsync(_client.BaseAddress + $"order/cart");
                if (responseData.IsSuccessStatusCode)
                {
                    var data = await responseData.Content.ReadAsStringAsync();
                    cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
                    HttpContext.Session.SetInt32("Cart", cartItem.Data.orderDetails.Count());
                }
                return Ok(new { success = true, message = "Item added to cart successfully." });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to add item to cart. Please try again." });
            }
        }


        [HttpGet] // Change to HttpPost to match the API controller
        public async Task<IActionResult> DeleteInCart([FromQuery] string itemId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, _client.BaseAddress + $"details/{itemId}");
            var jsonContent = JsonConvert.SerializeObject(itemId);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                UserCartResponse cartItem = new UserCartResponse();
                var responseData = await _client.GetAsync(_client.BaseAddress + $"order/cart");
                if (responseData.IsSuccessStatusCode)
                {
                    var data = await responseData.Content.ReadAsStringAsync();
                    cartItem = JsonConvert.DeserializeObject<UserCartResponse>(data);
                    HttpContext.Session.SetInt32("Cart", cartItem.Data.orderDetails.Count());
                }
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
