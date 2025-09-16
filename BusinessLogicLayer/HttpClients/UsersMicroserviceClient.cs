using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using System;

namespace BusinessLogicLayer.HttpClients
{
    public class UsersMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UsersMicroserviceClient> _logger;
        private readonly string _baseUrl;

        public UsersMicroserviceClient(HttpClient httpClient, IConfiguration configuration, ILogger<UsersMicroserviceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["Services:UsersMicroserviceBaseUrl"] ?? throw new ArgumentNullException("UsersMicroserviceBaseUrl");
        }

        public async Task<UserInfo?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var url = $"{_baseUrl}/api/Auth/{userId}";
                Console.WriteLine($"[OrdersService] Calling URL: {url}");

                var response = await _httpClient.GetAsync(url);
                //var response = await _httpClient.GetAsync($"{_baseUrl}/api/Auth/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserInfo>();
                }

                _logger.LogWarning("Failed to fetch user. Status Code: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while calling Users Microservice");
                throw;
            }
        }
    }

 
}
