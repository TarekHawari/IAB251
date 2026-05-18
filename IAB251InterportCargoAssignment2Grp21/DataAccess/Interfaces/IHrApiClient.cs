using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;

namespace IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces
{
    public interface IHrApiClient
    {

        Task<HrEmployeeDto?> GetEmployeeByEmailAsync(string email);
    }
}
