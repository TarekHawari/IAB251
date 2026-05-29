using System.Net.Http.Json;
using InterportCargo.QuotationService.Models;

namespace InterportCargo.QuotationService.Services
{
    public class NotificationServiceClient
    {
        private readonly HttpClient _httpClient;

        public NotificationServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("notification-api");
        }

        public async Task SendNotificationAsync(CreateNotificationRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/notifications", request);
            response.EnsureSuccessStatusCode();
        }
    }
}