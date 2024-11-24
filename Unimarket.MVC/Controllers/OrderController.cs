using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
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
            ResponseOrder cartItem2 = new ResponseOrder();
            var response = await _client.GetAsync(_client.BaseAddress + $"order/all?status={defaultSearch.InvoiceType}&page={defaultSearch.currentPage}&size={defaultSearch.perPage}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                cartItem2 = JsonConvert.DeserializeObject<ResponseOrder>(data);
                string status = "RECEIVED";
                response = await _client.GetAsync(_client.BaseAddress + $"order/all?status={status}&page={defaultSearch.currentPage}&size={defaultSearch.perPage}");
                data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<ResponseOrder>(data);
                ViewData["RECEIVED"] = cartItem.data.totalElements;
                status = "DELIVERY";
                response = await _client.GetAsync(_client.BaseAddress + $"order/all?status={status}&page={defaultSearch.currentPage}&size={defaultSearch.perPage}");
                data = await response.Content.ReadAsStringAsync();
                cartItem = JsonConvert.DeserializeObject<ResponseOrder>(data);
                ViewData["DELIVERY"] = cartItem.data.totalElements;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

            return View(cartItem2.data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderModel model)
        {
            HttpResponseMessage response = null;
            if (model.type == 0)
            {
                var status = new
                {
                    orderStatus = "DELIVERY",
                };
                response = await _client.PutAsync(_client.BaseAddress + $"order/status/{model.orderId}", new StringContent(
                 JsonConvert.SerializeObject(status),
                 Encoding.UTF8,
                 "application/json"));
            }
            else
            {
                var status = new
                {
                    orderStatus = "RECEIVED",
                };
                response = await _client.PutAsync(_client.BaseAddress + $"order/status/{model.orderId}", new StringContent(
                 JsonConvert.SerializeObject(status),
                 Encoding.UTF8,
                 "application/json"));
            }
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
