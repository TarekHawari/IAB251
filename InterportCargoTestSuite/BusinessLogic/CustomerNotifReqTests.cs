using Moq;
using Moq.Protected;
using Microsoft.EntityFrameworkCore;
using InterportCargo.QuotationService.Controllers;
using InterportCargo.QuotationService.Data;
using InterportCargo.QuotationService.Services;
using InterportCargo.QuotationService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace InterportCargoTestSuite.BusinessLogic
{
    [TestClass]
    public class CustomerNotifReqTests
    {
        private NotificationServiceClient _notificationServiceClient = default!;
        private QuotationDbContext dbContext = default!;
        private QuotationRequestsController _quotationRequestsController = default!;
        private CreateQuotationRequestDto _defaultRequest = default!;
        private Mock<HttpMessageHandler> _handlerMock = default!;

        [TestInitialize]
        public void TestInitialize()
        {
            // QuotationDbContext moq database
            dbContext = new QuotationDbContext(new DbContextOptionsBuilder<QuotationDbContext>().UseSqlite("Data Source=mock_database.db").Options);
            dbContext.Database.EnsureCreated();

            _handlerMock = new Mock<HttpMessageHandler>();
            _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockHttpClientFactory.Setup(f => f.CreateClient("notification-api"))
                .Returns(new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost/") });

            _notificationServiceClient = new NotificationServiceClient(mockHttpClientFactory.Object);

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

        [TestMethod]
        public async Task AcceptQuotation()
        {
            // Arrange
            var newRequest = _defaultRequest;

            // Act
            var savedRecord = await SubmitAndFetch(newRequest);
            var response = await _quotationRequestsController.Accept(savedRecord!.RequestId);
            var updatedRequest = await dbContext.QuotationRequests.FindAsync(savedRecord!.RequestId);

            // Assert
            Assert.IsNotNull(updatedRequest, "Request is not found in database.");
            Assert.AreEqual("Accepted", updatedRequest!.Status, "The status did not change to Accepted.");
        }

        [TestMethod]
        public async Task RejectQuotation()
        {
            // Arrange
            var newRequest = _defaultRequest;

            // Act
            var savedRecord = await SubmitAndFetch(newRequest);
            var rejectDto = new RejectQuotationRequestDto { RejectionMessage = "Rejected" };
            var response = await _quotationRequestsController.Reject(savedRecord!.RequestId, rejectDto);
            var updatedRequest = await dbContext.QuotationRequests.FindAsync(savedRecord!.RequestId);

            // Assert
            Assert.IsNotNull(updatedRequest, "Request is not found in database.");
            Assert.AreEqual("Rejected", updatedRequest!.Status);

            // checks if the notificatoin was sent to the customer
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), 
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }
    }
}