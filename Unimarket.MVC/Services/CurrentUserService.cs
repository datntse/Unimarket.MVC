using Newtonsoft.Json;
using Unimarket.MVC.Models;

namespace Unimarket.MVC.Services
{

    public interface ICurrentUserService
    {
        Task<UserModel> User();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;

        public CurrentUserService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _client = httpClientFactory.CreateClient("ServerApi");
            _client.BaseAddress = new Uri(configuration["Cron:localhost"]);

        }

        public async Task<UserModel> User()
        {
            var response = await _client.GetAsync(_client.BaseAddress + "User/GetUser");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(responseContent);
                return user;
            }
            else
            {
                return null;
            }
        }
    }

}
