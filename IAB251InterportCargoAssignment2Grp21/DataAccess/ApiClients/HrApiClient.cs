using System.Net.Http.Json;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces;

namespace IAB251InterportCargoAssignment2Grp21.DataAccess.ApiClients
{
    public class HrApiClient : IHrApiClient
    {


        private readonly HttpClient _httpClient;

        public HrApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();

            var baseUrl = configuration["HrApi:BaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("HR API BaseUrl is missing from appsettings.json.");
            }

            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<HrEmployeeDto?> GetEmployeeByEmailAsync(string email)
        {
            var employees = await _httpClient.GetFromJsonAsync<List<HrEmployeeDto>>("api/employees");

            if (employees == null)
            {
                return null;
            }

            return employees.FirstOrDefault(e =>
                e.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

    }
}
