using System.Net.Http.Json;
using IAB251InterportCargoAssignment2Grp21.Models;

namespace IAB251InterportCargoAssignment2Grp21.Services
{
    public class QuotationServiceClient
    {
        private readonly HttpClient _httpClient;

        public QuotationServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("quotation-api");
        }

        public async Task<List<QuotationRequestDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<QuotationRequestDto>>("api/quotationrequests")
                ?? new List<QuotationRequestDto>();
        }

        public async Task<List<QuotationRequestDto>> GetPendingAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<QuotationRequestDto>>("api/quotationrequests/pending")
                ?? new List<QuotationRequestDto>();
        }

        public async Task<List<QuotationRequestDto>> GetByCustomerIdAsync(int customerId)
        {
            return await _httpClient.GetFromJsonAsync<List<QuotationRequestDto>>($"api/quotationrequests/customer/{customerId}")
                ?? new List<QuotationRequestDto>();
        }

        public async Task<QuotationRequestDto?> GetByIdAsync(int requestId)
        {
            return await _httpClient.GetFromJsonAsync<QuotationRequestDto>($"api/quotationrequests/{requestId}");
        }

        public async Task CreateAsync(CreateQuotationRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/quotationrequests", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task AcceptAsync(int requestId)
        {
            var response = await _httpClient.PutAsync($"api/quotationrequests/{requestId}/accept", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    $"QuotationService returned {(int)response.StatusCode} {response.ReasonPhrase}. Body: {errorBody}");
            }
            response.EnsureSuccessStatusCode();
        }

        public async Task RejectAsync(int requestId, RejectQuotationRequestDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/quotationrequests/{requestId}/reject", dto);
            response.EnsureSuccessStatusCode();
        }
    }
}