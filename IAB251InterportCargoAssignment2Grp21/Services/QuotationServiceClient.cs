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

        public async Task<List<QuotationDto>> GetAllQuotationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<QuotationDto>>("api/quotations")
                ?? new List<QuotationDto>();
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

        public async Task<List<QuotationDto>> GetQuotationsByCustomerIdAsync(int customerId)
        {
            return await _httpClient.GetFromJsonAsync<List<QuotationDto>>($"api/quotations/customer/{customerId}")
                ?? new List<QuotationDto>();
        }

        public async Task<QuotationRequestDto?> GetByIdAsync(int requestId)
        {
            return await _httpClient.GetFromJsonAsync<QuotationRequestDto>($"api/quotationrequests/{requestId}");
        }

        public async Task<QuotationDto?> GetQuotationByIdAsync(int quotationId)
        {
            return await _httpClient.GetFromJsonAsync<QuotationDto>($"api/quotations/{quotationId}");
        }

        public async Task CreateAsync(CreateQuotationRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/quotationrequests", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<QuotationDto?> PrepareQuotationAsync(int quotationRequestId, PrepareQuotationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/quotations/prepare/{quotationRequestId}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"QuotationService returned {(int)response.StatusCode}: {body}");
            }

            return await response.Content.ReadFromJsonAsync<QuotationDto>();
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

        public async Task CustomerAcceptQuotationAsync(int quotationId)
        {
            var response = await _httpClient.PutAsync($"api/quotations/{quotationId}/customer-accept", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RejectAsync(int requestId, RejectQuotationRequestDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/quotationrequests/{requestId}/reject", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task CustomerRejectQuotationAsync(int quotationId)
        {
            var response = await _httpClient.PutAsync($"api/quotations/{quotationId}/customer-reject", null);
            response.EnsureSuccessStatusCode();
        }
    }
}