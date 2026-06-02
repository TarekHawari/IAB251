using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Services;
using IAB251InterportCargoAssignment2Grp21.DataAccess.Interfaces;
using IAB251InterportCargoAssignment2Grp21.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InterportCargoTestSuite.BusinessLogic
{
    [TestClass]
    public class EmployeeLoginServiceTests
    {
        private Mock<ILocalEmployeeCredentialRepository> _mockCredentialRepository = null!;
        private Mock<IHrApiClient> _mockHrApiClient = null!;
        private EmployeeLoginService _employeeLoginService = null!;



        [TestInitialize]
        public void TestInitialize()
        {
            _mockCredentialRepository = new Mock<ILocalEmployeeCredentialRepository>();
            _mockHrApiClient = new Mock<IHrApiClient>();

            _employeeLoginService = new EmployeeLoginService(
                _mockCredentialRepository.Object,
                _mockHrApiClient.Object
            );
        }

        [TestMethod]
        public async Task LoginAsync_ValidQuotationOfficer_ReturnsSuccessfulLogin()
        {
            // Arrange
            string email = "t.williams@company.com";
            string employeeKey = "QUOTE123";
            string hashedKey = BCrypt.Net.BCrypt.HashPassword(employeeKey);

            var localCredential = new LocalEmployeeCredential
            {
                LocalEmployeeCredentialId = 1,
                Email = email,
                EmployeeKey = hashedKey
            };

            var hrEmployee = new HrEmployeeDto
            {
                EmployeeId = 10,
                FirstName = "Tom",
                LastName = "Williams",
                Email = email,
                JobTitle = "Quotation Officer",
            };

            _mockCredentialRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns(localCredential);

            _mockHrApiClient
                .Setup(api => api.GetEmployeeByEmailAsync(email))
                .ReturnsAsync(hrEmployee);

            // Act
            var result = await _employeeLoginService.LoginAsync(email, employeeKey);

            // Assert
            Assert.IsTrue(result.IsSuccessful, "Valid quotation officer credentials should allow login.");
            Assert.AreEqual(email, result.EmployeeEmail);
            Assert.AreEqual("Quotation Officer", result.Role);
        }

        [TestMethod]
        public async Task LoginAsync_InvalidLocalEmployeeKey_ReturnsFailedLogin()
        {
            // Arrange
            string email = "t.williams@company.com";
            string correctKey = "QUOTE123";
            string enteredKey = "WRONG123";
            string hashedKey = BCrypt.Net.BCrypt.HashPassword(correctKey);

            var localCredential = new LocalEmployeeCredential
            {
                LocalEmployeeCredentialId = 1,
                Email = email,
                EmployeeKey = hashedKey
            };

            _mockCredentialRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns(localCredential);

            // Act
            var result = await _employeeLoginService.LoginAsync(email, enteredKey);

            // Assert
            Assert.IsFalse(result.IsSuccessful, "Incorrect employee key should fail login.");

            _mockHrApiClient.Verify(
                api => api.GetEmployeeByEmailAsync(It.IsAny<string>()),
                Times.Never,
                "HR API should not be called if local credential validation fails."
            );
        }

        [TestMethod]
        public async Task LoginAsync_EmployeeNotFoundInHrSystem_ReturnsFailedLogin()
        {
            // Arrange
            string email = "missing.employee@company.com";
            string employeeKey = "QUOTE123";
            string hashedKey = BCrypt.Net.BCrypt.HashPassword(employeeKey);

            var localCredential = new LocalEmployeeCredential
            {
                LocalEmployeeCredentialId = 2,
                Email = email,
                EmployeeKey = hashedKey
            };

            _mockCredentialRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns(localCredential);

            _mockHrApiClient
                .Setup(api => api.GetEmployeeByEmailAsync(email))
                .ReturnsAsync((HrEmployeeDto?)null);

            // Act
            var result = await _employeeLoginService.LoginAsync(email, employeeKey);

            // Assert
            Assert.IsFalse(result.IsSuccessful, "Employee should not login if HR API cannot find the employee.");
           
        }

        [TestMethod]
        public async Task LoginAsync_EmployeeIsNotQuotationOfficer_ReturnsFailedLogin()
        {
            // Arrange
            string email = "e.johnson@company.com";
            string employeeKey = "QUOTE123";
            string hashedKey = BCrypt.Net.BCrypt.HashPassword(employeeKey);

            var localCredential = new LocalEmployeeCredential
            {
                LocalEmployeeCredentialId = 3,
                Email = email,
                EmployeeKey = hashedKey
            };

            var hrEmployee = new HrEmployeeDto
            {
                EmployeeId = 5,
                FirstName = "Emma",
                LastName = "Johnson",
                Email = email,
                JobTitle = "Developer",
            };

            _mockCredentialRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns(localCredential);

            _mockHrApiClient
                .Setup(api => api.GetEmployeeByEmailAsync(email))
                .ReturnsAsync(hrEmployee);

            // Act
            var result = await _employeeLoginService.LoginAsync(email, employeeKey);

            // Assert
            Assert.IsFalse(result.IsSuccessful, "Only employees with the Quotation Officer role should be allowed to login.");
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.EmployeeEmail), "Failed login should not return an employee email.");
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.Role), "Failed login should not return an authorised role.");
        }

        [TestMethod]
        public async Task LoginAsync_EmailDoesNotExistInLocalCredentialStore_ReturnsFailedLogin()
        {
            // Arrange
            string email = "unknown@company.com";
            string employeeKey = "QUOTE123";

            _mockCredentialRepository
                .Setup(repo => repo.GetByEmail(email))
                .Returns((LocalEmployeeCredential?)null);

            // Act
            var result = await _employeeLoginService.LoginAsync(email, employeeKey);

            // Assert
            Assert.IsFalse(result.IsSuccessful, "Login should fail if no local employee credential exists.");

            _mockHrApiClient.Verify(
                api => api.GetEmployeeByEmailAsync(It.IsAny<string>()),
                Times.Never,
                "HR API should not be called when the local employee credential does not exist."
            );
        }


    }
}
