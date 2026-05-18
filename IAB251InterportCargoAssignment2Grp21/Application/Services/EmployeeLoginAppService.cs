using IAB251InterportCargoAssignment2Grp21.Application.Interfaces;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Interfaces;

namespace IAB251InterportCargoAssignment2Grp21.Application.Services
{
    public class EmployeeLoginAppService : IEmployeeLoginAppService
    {

        private readonly IEmployeeLoginService _employeeLoginService;

        public EmployeeLoginAppService(IEmployeeLoginService employeeLoginService)
        {
            _employeeLoginService = employeeLoginService;
        }

        public async Task<EmployeeLoginResult> LoginAsync(string email, string employeeKey)
        {
            return await _employeeLoginService.LoginAsync(email, employeeKey);
        }
    }
}
