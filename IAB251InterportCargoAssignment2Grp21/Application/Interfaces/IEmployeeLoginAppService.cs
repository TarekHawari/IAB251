
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
namespace IAB251InterportCargoAssignment2Grp21.Application.Interfaces
{
    public interface IEmployeeLoginAppService
    {

        Task<EmployeeLoginResult> LoginAsync(string email, string employeeKey);
    }
}
