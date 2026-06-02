using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IAB251InterportCargoAssignment2Grp21.Application.Services;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InterportCargoTestSuite.Application
{
    [TestClass]
    public class EmployeeLoginAppServiceTests
    {
        private Mock<IEmployeeLoginService> _mockEmployeeLoginService = null!;
        private EmployeeLoginAppService _employeeLoginAppService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEmployeeLoginService = new Mock<IEmployeeLoginService>();
            _employeeLoginAppService = new EmployeeLoginAppService(_mockEmployeeLoginService.Object);
        }


        [TestMethod]
        public async Task LoginAsync_ValidInput_ReturnsSuccessfulLoginResult()
        {
            // Arrange
            string email = "t.williams@company.com";
            string employeeKey = "QUOTE123";

            var expectedResult = new EmployeeLoginResult 
            {
                IsSuccessful = true,
                EmployeeName = "Tom Williams",
                EmployeeEmail = "t.williams@company.com",
                Role = "Quotation Officer"
            };

            _mockEmployeeLoginService
                .Setup(service => service.LoginAsync(email, employeeKey))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _employeeLoginAppService.LoginAsync(email, employeeKey);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual("Tom Williams", result.EmployeeName);
            Assert.AreEqual("t.williams@company.com", result.EmployeeEmail);
            Assert.AreEqual("Quotation Officer", result.Role);

            _mockEmployeeLoginService.Verify(
                service => service.LoginAsync(email, employeeKey),
                Times.Once
            );
        }



        [TestMethod]
        public async Task LoginAsync_InvalidInput_ReturnsFailedLoginResult()
        {
            // Arrange
            string email = "wrong@company.com";
            string employeeKey = "BADKEY";

            var expectedResult = new EmployeeLoginResult
            {
                IsSuccessful = false,
                EmployeeName = string.Empty,
                EmployeeEmail = string.Empty,
                Role = string.Empty
            };

            _mockEmployeeLoginService
                .Setup(service => service.LoginAsync(email, employeeKey))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _employeeLoginAppService.LoginAsync(email, employeeKey);

            // Assert
            Assert.IsFalse(result.IsSuccessful);

            _mockEmployeeLoginService.Verify(
                service => service.LoginAsync(email, employeeKey),
                Times.Once
            );
        }

    }
}
