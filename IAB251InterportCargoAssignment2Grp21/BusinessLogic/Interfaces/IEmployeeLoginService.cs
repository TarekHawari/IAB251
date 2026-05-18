using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;


namespace IAB251InterportCargoAssignment2Grp21.BusinessLogic.Interfaces
{
    public interface IEmployeeLoginService
    {
        Task<EmployeeLoginResult> LoginAsync(string email, string employeeKey);
    }
}
