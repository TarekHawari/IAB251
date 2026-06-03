using Moq;
using Microsoft.EntityFrameworkCore;
using InterportCargo.QuotationService.Controllers;
using InterportCargo.QuotationService.Data;
using InterportCargo.QuotationService.Services;
using InterportCargo.QuotationService.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterportCargoTestSuite.BusinessLogic
{
    [TestClass]
    public class CustomerQuoteReqUnitTests
    {
        private NotificationServiceClient _notificationServiceClient = default!;
        private QuotationDbContext dbContext = default!;
        private QuotationRequestsController _quotationRequestsController = default!;
        private CreateQuotationRequestDto _defaultRequest = default!;

        [TestInitialize]
        public void TestInitialize()
        {   
            // QuotationDbContext moq database
            dbContext = new QuotationDbContext(new DbContextOptionsBuilder<QuotationDbContext>().UseSqlite("Data Source=mock_database.db").Options);
            dbContext.Database.EnsureCreated();

            _quotationRequestsController = new QuotationRequestsController(dbContext, _notificationServiceClient);

            _defaultRequest = new CreateQuotationRequestDto
            {
                CustomerId = 11,
                CustomerName = "test",
                CustomerEmail = "test@gmail.com",
                Source = "Australia",
                Destination = "China",
                NumberOfContainers = 10,
                GoodsType = "Machinery",
                PackageWidth = 1800.0m,
                PackageHeight = 2000.0m,
                ImportExportType = "Import",
                PackingType = "Packing",
                RequiresQuarantine = true,
                QuarantineDetails = "Quarantined",
                RequiresFumigation = true,
                FumigationDetails = "Fumigated",
            };
        }
        
        private async Task<QuotationRequest?> SubmitAndFetch(CreateQuotationRequestDto request)
        {
            var response = await _quotationRequestsController.Create(request);

            if (response.Result is not CreatedAtActionResult { Value: QuotationRequest created })
                return null;  

            return await dbContext.QuotationRequests.FindAsync(created.RequestId);
        }

        // check if controller is can accept requests
        [TestMethod]
        public async Task quotationCheckController()
        {           
            // Arrange
            var newRequest = _defaultRequest;
            
            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var createdResult = response.Result as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(createdResult, "The controller did not return successfully");
        }

        // check if the quotation request was inserted correctly
        [TestMethod]
        public async Task quotationRequestSubmitAndVerifyIfInserted()
        {
            // Arrange
            var newRequest = _defaultRequest;

            // Act
            var savedRecord = await SubmitAndFetch(newRequest);

            // Assert
            Assert.IsNotNull(savedRecord, "Failed to find the inserted quotation request.");
        }

        // insert the new quotation request and verify the payload inserted is correct
        [TestMethod]
        public async Task QuotationRequestSubmitVerifyValidPayload()
        {
            // Special insert with no Quarantine and no Fumigation
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.RequiresQuarantine = false;
            newRequest.QuarantineDetails = "";
            newRequest.RequiresFumigation = false;
            newRequest.FumigationDetails = "";

            // Act
            var savedRecord = await SubmitAndFetch(newRequest);

            // Assert
            Assert.AreEqual(newRequest.CustomerId, savedRecord!.CustomerId);
            Assert.AreEqual(newRequest.CustomerName, savedRecord.CustomerName);
            Assert.AreEqual(newRequest.CustomerEmail, savedRecord.CustomerEmail);
            Assert.AreEqual(newRequest.Source, savedRecord.Source);
            Assert.AreEqual(newRequest.Destination, savedRecord.Destination);
            Assert.AreEqual(newRequest.NumberOfContainers, savedRecord.NumberOfContainers);
            Assert.AreEqual(newRequest.GoodsType, savedRecord.GoodsType);
            Assert.AreEqual(newRequest.PackageWidth, savedRecord.PackageWidth);
            Assert.AreEqual(newRequest.PackageHeight, savedRecord.PackageHeight);
            Assert.AreEqual(newRequest.ImportExportType, savedRecord.ImportExportType);
            Assert.AreEqual(newRequest.PackingType, savedRecord.PackingType);
            Assert.AreEqual(newRequest.RequiresQuarantine, savedRecord.RequiresQuarantine);
            Assert.AreEqual(newRequest.RequiresFumigation, savedRecord.RequiresFumigation);
            Assert.AreEqual( string.IsNullOrEmpty(newRequest.QuarantineDetails) ? null : newRequest.QuarantineDetails, savedRecord.QuarantineDetails);
            Assert.AreEqual( string.IsNullOrEmpty(newRequest.FumigationDetails) ? null : newRequest.FumigationDetails, savedRecord.FumigationDetails);
        }

        // insert the quotation request and verify if set to pending
        [TestMethod]
        public async Task QuotationRequestSubmitInitialStatusIsPending()
        {
            // Arrange
            var newRequest = _defaultRequest;

            // Act
            var savedRecord = await SubmitAndFetch(newRequest);

            // Assert
            Assert.AreEqual("Pending", savedRecord!.Status, "The status of quotation request is not Pending.");
            //Console.WriteLine("The initial status of quotation request is Pending.");
        }

        // the following is to check if any of the required fields are missing
        [TestMethod]
        public async Task QuotationRequestSubmit_MissingSource_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.Source = string.Empty;
            
            _quotationRequestsController.ModelState.AddModelError("Source", "Source is required.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Source is required.", ((string[])errors!["Source"])[0]);
        }

        [TestMethod]
        public async Task QuotationRequestSubmit_MissingDestination_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.Destination = string.Empty;
            
            _quotationRequestsController.ModelState.AddModelError("Destination", "Destination is required.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Destination is required.", ((string[])errors!["Destination"])[0]);
        }

        [TestMethod]
        public async Task QuotationRequestSubmit_MissingGoodsType_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.GoodsType = string.Empty;
            
            _quotationRequestsController.ModelState.AddModelError("GoodsType", "Goods type is required.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Goods type is required.", ((string[])errors!["GoodsType"])[0]);
        }

        [TestMethod]
        public async Task QuotationRequestSubmit_MissingImportExportType_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.ImportExportType = string.Empty;
            
            _quotationRequestsController.ModelState.AddModelError("ImportExportType", "Import/export type is required.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Import/export type is required.", ((string[])errors!["ImportExportType"])[0]);
        }

        [TestMethod]
        public async Task QuotationRequestSubmit_MissingPackingType_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.PackingType = string.Empty;
            
            _quotationRequestsController.ModelState.AddModelError("PackingType", "Packing type is required.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Packing type is required.", ((string[])errors!["PackingType"])[0]);
        }

        // the following is to check if any of the range fields are correct
        [TestMethod]
        public async Task QuotationRequestSubmit_NumberOfContainersBelowMin_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.NumberOfContainers = 0; 
            
            _quotationRequestsController.ModelState.AddModelError("NumberOfContainers", "Number of containers must be at least 1.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Number of containers must be at least 1.", ((string[])errors!["NumberOfContainers"])[0]);
        }

        [TestMethod]
        public async Task QuotationRequestSubmit_PackageWidthBelowMin_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.PackageWidth = 0m; 
            
            _quotationRequestsController.ModelState.AddModelError("PackageWidth", "Package width must be entered in millimetres and must be greater than 0.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Package width must be entered in millimetres and must be greater than 0.", ((string[])errors!["PackageWidth"])[0]);
        }

        [TestMethod]
        public async Task QuotationRequestSubmit_PackageHeightBelowMin_ReturnsBadRequest()
        {
            // Arrange
            var newRequest = _defaultRequest;
            newRequest.PackageHeight = 0m; 
            
            _quotationRequestsController.ModelState.AddModelError("PackageHeight", "Package height must be entered in millimetres and must be greater than 0.");

            // Act
            var response = await _quotationRequestsController.Create(newRequest);
            var badRequestResult = response.Result as BadRequestObjectResult;
            var errors = badRequestResult!.Value as SerializableError;

            // Assert
            Assert.AreEqual("Package height must be entered in millimetres and must be greater than 0.", ((string[])errors!["PackageHeight"])[0]);
        }
    }
}