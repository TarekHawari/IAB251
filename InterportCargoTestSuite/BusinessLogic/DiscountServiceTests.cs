using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterportCargo.QuotationService.Models;
using InterportCargo.QuotationService.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterportCargoTestSuite.BusinessLogic
{
    [TestClass]
    public class DiscountServiceTests
    {
        private DiscountService _discountService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _discountService = new DiscountService();
        }

        [TestMethod]
        public void GetEligibleDiscountPercentage_ContainersFiveWithQuarantine_ReturnsZero()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 5,
                RequiresQuarantine = true,
                RequiresFumigation = false
            };

            // Act
            var result = _discountService.GetEligibleDiscountPercentage(request);

            // Assert
            Assert.AreEqual(0m, result, "Exactly 5 containers should not qualify for a discount.");
        }

        [TestMethod]
        public void GetEligibleDiscountPercentage_ContainersSixWithQuarantineOnly_ReturnsTwoPointFive()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 6,
                RequiresQuarantine = true,
                RequiresFumigation = false
            };

            // Act
            var result = _discountService.GetEligibleDiscountPercentage(request);

            // Assert
            Assert.AreEqual(2.5m, result, "More than 5 containers with quarantine should qualify for 2.5% discount.");
        }

        [TestMethod]
        public void GetEligibleDiscountPercentage_ContainersSixWithFumigationOnly_ReturnsTwoPointFive()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 6,
                RequiresQuarantine = false,
                RequiresFumigation = true
            };

            // Act
            var result = _discountService.GetEligibleDiscountPercentage(request);

            // Assert
            Assert.AreEqual(2.5m, result, "More than 5 containers with fumigation should qualify for 2.5% discount.");
        }

        [TestMethod]
        public void GetEligibleDiscountPercentage_ContainersSixWithQuarantineAndFumigation_ReturnsFive()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 6,
                RequiresQuarantine = true,
                RequiresFumigation = true
            };

            // Act
            var result = _discountService.GetEligibleDiscountPercentage(request);

            // Assert
            Assert.AreEqual(5m, result, "More than 5 containers with both quarantine and fumigation should qualify for 5% discount.");
        }

        [TestMethod]
        public void GetEligibleDiscountPercentage_ContainersTenWithQuarantineAndFumigation_ReturnsFive()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 10,
                RequiresQuarantine = true,
                RequiresFumigation = true
            };

            // Act
            var result = _discountService.GetEligibleDiscountPercentage(request);

            // Assert
            Assert.AreEqual(5m, result, "Exactly 10 containers should not qualify for the 10% discount.");
        }

        [TestMethod]
        public void GetEligibleDiscountPercentage_ContainersElevenWithQuarantineAndFumigation_ReturnsTen()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 11,
                RequiresQuarantine = true,
                RequiresFumigation = true
            };

            // Act
            var result = _discountService.GetEligibleDiscountPercentage(request);

            // Assert
            Assert.AreEqual(10m, result, "More than 10 containers with both quarantine and fumigation should qualify for 10% discount.");
        }

        [TestMethod]
        public void GetEligibleDiscountPercentage_ContainersElevenWithoutQuarantineOrFumigation_ReturnsZero()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 11,
                RequiresQuarantine = false,
                RequiresFumigation = false
            };

            // Act
            var result = _discountService.GetEligibleDiscountPercentage(request);

            // Assert
            Assert.AreEqual(0m, result, "Container count alone should not qualify for a discount.");
        }
    }
}