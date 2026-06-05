using IAB251InterportCargoAssignment2Grp21.Data;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Repositories;
using IAB251InterportCargoAssignment2Grp21.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterportCargoTestSuite.DataAccess
{
    [TestClass]
    public class EFEmployeeCredentialRepositoryTests
    {
        private SqliteConnection _connection = null!;
        private InterportCargoContext _context = null!;
        private EFEmployeeCredentialRepository _repository = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<InterportCargoContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new InterportCargoContext(options);
            _context.Database.EnsureCreated();

            _repository = new EFEmployeeCredentialRepository(_context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    
        [TestMethod]
        public void GetByEmail_ExistingEmployeeCredential_ReturnsCredential() // When a matching employee email exists in the Employee table, the repository returns the correct credential.
        {
            // Arrange
            var credential = new LocalEmployeeCredential
            {
                LocalEmployeeCredentialId = 100,
                Email = "t.williams@company.com",
                EmployeeKey = BCrypt.Net.BCrypt.HashPassword("QUOTE123")
            };

            _context.LocalEmployeeCredentials.Add(credential);
            _context.SaveChanges();

            // Act
            var result = _repository.GetByEmail("t.williams@company.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("t.williams@company.com", result.Email);
        }

        [TestMethod]
        public void GetByEmail_UnknownEmployeeCredential_ReturnsNull() // When no matching employee email exists, the repository correctly returns null.
        {
            // Arrange
            var credential = new LocalEmployeeCredential
            {
                LocalEmployeeCredentialId = 101,
                Email = "t.williams@company.com",
                EmployeeKey = BCrypt.Net.BCrypt.HashPassword("QUOTE123")
            };

            _context.LocalEmployeeCredentials.Add(credential);
            _context.SaveChanges();

            // Act
            var result = _repository.GetByEmail("unknown@company.com");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetByEmail_EmailCaseDiffers_ReturnsCredential() // This Test checks whether the repository handles email case safely, the expected output should return true. 
        {
            // Arrange
            var credential = new LocalEmployeeCredential
            {
                LocalEmployeeCredentialId = 102,
                Email = "t.williams@company.com",
                EmployeeKey = BCrypt.Net.BCrypt.HashPassword("QUOTE123")
            };

            _context.LocalEmployeeCredentials.Add(credential);
            _context.SaveChanges();

            // Act
            var result = _repository.GetByEmail("T.WILLIAMS@COMPANY.COM");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("t.williams@company.com", result.Email);
        }
    }
}
