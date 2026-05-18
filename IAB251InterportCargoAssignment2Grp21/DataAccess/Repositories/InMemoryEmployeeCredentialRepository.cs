
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces;

namespace IAB251InterportCargoAssignment2Grp21.DataAccess.Repositories
{
    public class InMemoryEmployeeCredentialRepository : ILocalEmployeeCredentialRepository
    {

        private readonly List<LocalEmployeeCredential> _credentials = new()
        {

            //Real Employee stays (Quotation Officer)
            new LocalEmployeeCredential
            {
                Email = "t.williams@company.com",
                EmployeeKey = "QUOTE123"
            },

            // Fake Employee
            new LocalEmployeeCredential
            {
                Email = "fake.employee@company.com",
                EmployeeKey = "FAKE123"
            },


            new LocalEmployeeCredential
            {
                 Email = "s.mitchell@company.com",
                 EmployeeKey = "SALES123"
            }

        };

        public LocalEmployeeCredential? GetByEmail(string email)
        {
            return _credentials.FirstOrDefault(c =>
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

    }
}
