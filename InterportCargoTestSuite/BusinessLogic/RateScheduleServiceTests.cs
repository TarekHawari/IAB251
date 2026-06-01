using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterportCargo.QuotationService.Models;
using InterportCargo.QuotationService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterportCargoTestSuite.BusinessLogic
{
   [TestClass]
    public class RateScheduleServiceTests
    {
        private RateScheduleService _rateScheduleService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _rateScheduleService = new RateScheduleService();
        }

        [TestMethod]
        public void GetRates_ContainerType20Feet_ReturnsCorrectWharfBookingFee()
        {
            // Arrange
            string containerType = "20 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(60m, result.WharfBookingFee, "20 Feet container should have a Wharf Booking Fee of 60.");
        }

        [TestMethod]
        public void GetRates_ContainerType20Feet_ReturnsCorrectLclDeliveryDepotFee()
        {
            // Arrange
            string containerType = "20 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(400m, result.LclDeliveryDepotFee, "20 Feet container should have an LCL Delivery Depot Fee of 400.");
        }

        [TestMethod]
        public void GetRates_ContainerType20Feet_ReturnsCorrectGstRate()
        {
            // Arrange
            string containerType = "20 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(0.10m, result.GstRate, "GST rate should be 10%.");
        }

        [TestMethod]
        public void GetRates_ContainerType40Feet_ReturnsCorrectWharfBookingFee()
        {
            // Arrange
            string containerType = "40 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(70m, result.WharfBookingFee, "40 Feet container should have a Wharf Booking Fee of 70.");
        }

        [TestMethod]
        public void GetRates_ContainerType40Feet_ReturnsCorrectLclDeliveryDepotFee()
        {
            // Arrange
            string containerType = "40 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(500m, result.LclDeliveryDepotFee, "40 Feet container should have an LCL Delivery Depot Fee of 500.");
        }

        [TestMethod]
        public void GetRates_ContainerType40Feet_ReturnsCorrectGstRate()
        {
            // Arrange
            string containerType = "40 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(0.10m, result.GstRate, "GST rate should be 10%.");
        }

        [TestMethod]
        public void GetRates_ContainerType20Feet_ReturnsAllExpectedCharges()
        {
            // Arrange
            string containerType = "20 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(60m, result.WharfBookingFee);
            Assert.AreEqual(80m, result.LiftOnLiftOffFee);
            Assert.AreEqual(220m, result.FumigationFee);
            Assert.AreEqual(400m, result.LclDeliveryDepotFee);
            Assert.AreEqual(120m, result.TailgateInspectionFee);
            Assert.AreEqual(240m, result.StorageFee);
            Assert.AreEqual(70m, result.FacilityFee);
            Assert.AreEqual(60m, result.WharfInspectionFee);
            Assert.AreEqual(0.10m, result.GstRate);
        }

        [TestMethod]
        public void GetRates_ContainerType40Feet_ReturnsAllExpectedCharges()
        {
            // Arrange
            string containerType = "40 Feet";

            // Act
            RateSchedule result = _rateScheduleService.GetRates(containerType);

            // Assert
            Assert.AreEqual(70m, result.WharfBookingFee);
            Assert.AreEqual(120m, result.LiftOnLiftOffFee);
            Assert.AreEqual(280m, result.FumigationFee);
            Assert.AreEqual(500m, result.LclDeliveryDepotFee);
            Assert.AreEqual(160m, result.TailgateInspectionFee);
            Assert.AreEqual(300m, result.StorageFee);
            Assert.AreEqual(100m, result.FacilityFee);
            Assert.AreEqual(90m, result.WharfInspectionFee);
            Assert.AreEqual(0.10m, result.GstRate);
        }

        [TestMethod]
        public void GetRates_InvalidContainerType_ThrowsArgumentException()
        {
            // Arrange
            string containerType = "10 Feet";
            bool exception = false;

            // Act and Assert
            try
            {
                _rateScheduleService.GetRates(containerType);

            }
            catch (ArgumentException)
            {
                // Assert
                exception = true;
            }

            // Assert
            Assert.IsTrue(exception, "Invalid container type should throw an ArgumentException.");
        }
    }
}
