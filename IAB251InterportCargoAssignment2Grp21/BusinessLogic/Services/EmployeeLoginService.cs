using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Interfaces;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces;

namespace IAB251InterportCargoAssignment2Grp21.BusinessLogic.Services
{
    public class EmployeeLoginService : IEmployeeLoginService
    {

        private readonly ILocalEmployeeCredentialRepository _credentialRepository;
        private readonly IHrApiClient _hrApiClient;

        public EmployeeLoginService(
            ILocalEmployeeCredentialRepository credentialRepository,
            IHrApiClient hrApiClient)
        {
            _credentialRepository = credentialRepository;
            _hrApiClient = hrApiClient;
        }

        public async Task<EmployeeLoginResult> LoginAsync(string email, string employeeKey)
        {
            var localCredential = _credentialRepository.GetByEmail(email);

            if (localCredential == null)
            {
                return Failed();
            }

            if (!localCredential.EmployeeKey.Equals(employeeKey, StringComparison.Ordinal))
            {
                return Failed();
            }

            var hrEmployee = await _hrApiClient.GetEmployeeByEmailAsync(email);

            if (hrEmployee == null)
            {
                return Failed();
            }

            if (!IsQuotationOfficer(hrEmployee.JobTitle))
            {
                return Failed();
            }

            return new EmployeeLoginResult
            {
                IsSuccessful = true,
                EmployeeEmail = hrEmployee.Email,
                EmployeeName = $"{hrEmployee.FirstName} {hrEmployee.LastName}",
                Role = hrEmployee.JobTitle
            };
        }

        private static bool IsQuotationOfficer(string jobTitle)
        {
            var normalisedRole = jobTitle.Replace(" ", "").Trim().ToLowerInvariant();

            return normalisedRole == "quotationofficer";
        }

        private static EmployeeLoginResult Failed()
        {
            return new EmployeeLoginResult
            {
                IsSuccessful = false
            };
        }

    }
}
