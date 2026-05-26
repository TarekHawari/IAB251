using System.Net.Http.Json;
using IAB251InterportCargoAssignment2Grp21.Models;

namespace IAB251InterportCargoAssignment2Grp21.Services
{
    public class NotificationServiceClient
    {
        private readonly HttpClient _httpClient;

        public NotificationServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("notification-api");
        }

        public async Task<List<CustomerNotificationDto>> GetByCustomerIdAsync(int customerId)
        {
            return await _httpClient.GetFromJsonAsync<List<CustomerNotificationDto>>($"api/notifications/customer/{customerId}")
                ?? new List<CustomerNotificationDto>();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var response = await _httpClient.PutAsync($"api/notifications/{notificationId}/read", null);
            response.EnsureSuccessStatusCode();
        }
    }
}