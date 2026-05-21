using IAB251InterportCargoAssignment2Grp21.Models;


namespace IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces
{
    public interface ILocalEmployeeCredentialRepository
    {

        LocalEmployeeCredential? GetByEmail(string email);
    }
}
