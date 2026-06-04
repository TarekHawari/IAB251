using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Data;
using IAB251InterportCargoAssignment2Grp21.Pages.Account;

namespace InterportCargoTestSuite.BusinessLogic
{
    [TestClass]
    public class CustomerAuthTests
    {
        private InterportCargoContext _dbContext = default!;
        private CustomerLoginModel _loginModel = default!;
        private CustomerRegisterModel _registerModel = default!;

        [TestInitialize]
        public void TestInitialize()
        {
            // create a memory database moq of the InterportCargoContext
            _dbContext = new InterportCargoContext(new DbContextOptionsBuilder<InterportCargoContext>().UseSqlite("DataSource=:memory:").Options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _dbContext.Customers.Add(new Customer
            {
                FirstName = "test",
                FamilyName = "test",
                Email = "test@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("a customer password to be encrypted"),
                Phone = "0412345678",
                Address = "123 street building 456, a city"
            });
            _dbContext.SaveChanges();

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new Mock<ISession>().Object;
            var pageContext = new PageContext { HttpContext = httpContext };

            _loginModel = new CustomerLoginModel(_dbContext);
            _loginModel.PageContext = pageContext;

            _registerModel = new CustomerRegisterModel(_dbContext);
            _registerModel.PageContext = pageContext;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Dispose();
        }

        [TestMethod]
        public void OnPostCustomerLoginValidCustomerRedirectsToDashboard()
        {
            // Arrange
            _loginModel.customer = new Customer
            {
                Email = "test@gmail.com",
                Password = "a customer password to be encrypted"
            };

            // Act
            var result = _loginModel.OnPostCustomerLogin();

            // Assert
            var redirectResult = (RedirectToPageResult)result;
            Assert.AreEqual("/Quotes/CustomerDashboard", redirectResult.PageName);
        }
        [TestMethod]
        public void OnPostCustomerLoginWrongPasswordShowError()
        {
            // Arrange
            _loginModel.customer = new Customer
            {
                Email = "test@gmail.com",
                Password = "a wrong customer password"
            };

            // Act
            var result = _loginModel.OnPostCustomerLogin();

            // Assert
            Assert.IsFalse(_loginModel.ModelState.IsValid);
            Assert.IsTrue(_loginModel.ModelState.ContainsKey("customer.Password"));
            Assert.AreEqual("Invalid Password", _loginModel.ModelState["customer.Password"]!.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void OnPostCustomerLoginWrongEmailShowErrors()
        {
            // Arrange
            _loginModel.customer = new Customer
            {
                Email = "nobody@test.com",
                Password = "SomePassword"
            };

            // Act
            var result = _loginModel.OnPostCustomerLogin();

            // Assert
            Assert.IsFalse(_loginModel.ModelState.IsValid);
            Assert.IsTrue(_loginModel.ModelState.ContainsKey("customer.Email"));
            Assert.AreEqual("Email does not exist", _loginModel.ModelState["customer.Email"]!.Errors[0].ErrorMessage);
        }

        // Registration 
        [TestMethod]
        public void OnPostCustomerRegisterSavesCustomerToDatabase()
        {
            // Arrange
            _registerModel.customer = new Customer
            {
                FirstName = "test2",
                FamilyName = "test2",
                Email = "test222@gmail.com",
                Password = "Password2",
                Phone = "04987465632",
                Address = "111 Street"
            };
            _registerModel.ModelState.Clear();

            // Act
            _registerModel.OnPostCustomerRegister();

            // Assert
            var saved = _dbContext.Customers.FirstOrDefault(c => c.Email == "test222@gmail.com");
            Assert.IsNotNull(saved, "Customer should be inserted into the database.");
        }

        [TestMethod]
        public void OnPostCustomerRegisterPasswordIsHashed()
        {
            // Arrange
            _registerModel.customer = new Customer
            {
                FirstName = "test2",
                FamilyName = "test2",
                Email = "test222@gmail.com",
                Password = "password",
                Phone = "04987456632",
                Address = "111 Street"
            };
            _registerModel.ModelState.Clear();

            // Act
            _registerModel.OnPostCustomerRegister();
            var saved = _dbContext.Customers.AsNoTracking().FirstOrDefault(c => c.Email == "test222@gmail.com");

            // Assert
            Assert.IsNotNull(saved);
            Assert.AreNotEqual("password", saved.Password, "Password in plain text.");
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("password", saved.Password), "password is BCrypt hash.");
        }

        [TestMethod]
        public void OnPostCustomerRegisterEmailExistReturnsPageWithError()
        {
            // Arrange
            _registerModel.customer = new Customer
            {
                FirstName = "test2",
                FamilyName = "test2",
                Email = "test@gmail.com",
                Password = "password",
                Phone = "04987456632",
                Address = "111 Street"
            };
            _registerModel.ModelState.Clear();

            // Act
            var result = _registerModel.OnPostCustomerRegister();

            // Assert
            Assert.IsTrue(_registerModel.ModelState.ContainsKey("customer.Email"));
            Assert.AreEqual("Email already exists", _registerModel.ModelState["customer.Email"]!.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void OnPostCustomerRegisterRegisterRedirectsToLoginPage()
        {
            // Arrange
            _registerModel.customer = new Customer
            {
                FirstName = "test2",
                FamilyName = "test2",
                Email = "test222@gmail.com",
                Password = "Password2",
                Phone = "04987465632",
                Address = "111 Street"
            };
            _registerModel.ModelState.Clear();

            // Act
            var result = _registerModel.OnPostCustomerRegister();

            // Assert
            Assert.AreEqual("/Account/CustomerLogin", ((RedirectToPageResult)result).PageName);
        }
    }
}