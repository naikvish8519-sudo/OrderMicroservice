using BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.HttpClients
{
    public class ProductMicroserviceClient
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<UsersMicroserviceClient> _logger;
        private readonly string _baseUrl;

        public ProductMicroserviceClient(HttpClient httpClient, IConfiguration configuration, ILogger<UsersMicroserviceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["Services:ProductMicroserviceBaseUrl"] ?? throw new ArgumentNullException("UsersMicroserviceBaseUrl");
        }

        public async Task<ProductInfo?> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var url = $"{_baseUrl}/api/products/search/product-id/{productId}";
               

                var response = await _httpClient.GetAsync(url);
                //var response = await _httpClient.GetAsync($"{_baseUrl}/api/Auth/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProductInfo>();
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

