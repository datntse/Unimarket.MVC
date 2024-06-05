using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Unimarket.MVC.Helpers;
using Unimarket.MVC.Models.ViewModels;
using Unimarket.MVC.Services;

namespace Unimarket.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<OrderController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrentUserService _currentUserService;
        public OrderController(ILogger<OrderController> logger, IHttpClientFactory httpClientFactory,
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
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrder order)
        {
            UpdateOrder _order = new UpdateOrder
            {
                OrderId = order.OrderId,
                Status = order.Status
            };

            var response = await _client.PutAsync(_client.BaseAddress + "Order/update/order", new StringContent(
                JsonConvert.SerializeObject(_order),
                Encoding.UTF8,
                "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true, message = "Order updated successfully!" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to update order. Please try again." });
            }
        }

    }
}
