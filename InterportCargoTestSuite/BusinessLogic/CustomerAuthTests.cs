// using Moq;
// using Microsoft.EntityFrameworkCore;
// using InterportCargo.QuotationService.Controllers;
// using InterportCargo.QuotationService.Data;
// using InterportCargo.QuotationService.Services;
// using InterportCargo.QuotationService.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace InterportCargoTestSuite.BusinessLogic
// {
//     [TestClass]
//     public class CustomerAuthTests
//     {   
//         private CustomerRegisterModel _customerRegisterModel = default!;
//         private CustomerLoginModel _customerLoginModel = default!;


//         [TestInitialize]
//         public void TestInitialize()
//         {
//             _mockCredentialRepository = new Mock<ILocalEmployeeCredentialRepository>();
//             _mockHrApiClient = new Mock<IHrApiClient>();

//             _employeeLoginService = new EmployeeLoginService(
//                 _mockCredentialRepository.Object,
//                 _mockHrApiClient.Object
//             );
//         }

//         // register successful
        
//         // register error handling

//         // login sucessful

//         // login not successful
        
//     }
// }