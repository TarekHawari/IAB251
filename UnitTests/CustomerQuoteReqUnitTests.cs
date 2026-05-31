using IAB251InterportCargoAssignment2Grp21.Services;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;

namespace UnitTests.CustomerQuoteReqUnitTests
{
    [TestClass]
    public class CustomerQuoteReqUnitTests
    {
        private Mock<IHttpClientFactory> mock;
        private QuotationServiceClient quoteServiceClient;

        public CustomerQuoteReqUnitTests() {
            
        }

        [TestMethod]
        public void getCustomerQuoteTest() {
            int customerId = 1;
            mock = new Mock<IHttpClientFactory>();
            quoteServiceClient = new QuotationServiceClient(mock.Object);
        
            Assert.IsNotNull(quoteServiceClient.GetQuotationsByCustomerIdAsync(customerId));
        }
    }
}

