using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
using IAB251InterportCargoAssignment2Grp21.Data;
using IAB251InterportCargoAssignment2Grp21.DataAccess;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces;
using IAB251InterportCargoAssignment2Grp21.Models;

namespace IAB251InterportCargoAssignment2Grp21.DataAccess.Repositories
{
    public class EFEmployeeCredentialRepository : ILocalEmployeeCredentialRepository
    {
        private readonly InterportCargoContext _context;

        public EFEmployeeCredentialRepository(InterportCargoContext context)
        {
            _context = context;
        }

        public LocalEmployeeCredential? GetByEmail(string email)
        {
            return _context.LocalEmployeeCredentials
                .FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
        }
    }
}