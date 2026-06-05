using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Xunit;

namespace IAB251InterportCargoAssignment2Grp21.Tests
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _discountService;

        public DiscountServiceTests()
        {
            _discountService = new DiscountService();
        }

        // ── DISCOUNT ELIGIBILITY TESTS ──

        [Fact]
        public void CheckDiscount_ThreeOrMoreContainers_ReturnsFivePercent()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 3,
                RequiresQuarantine = false,
                RequiresPacking = false,
                RequiresUnpacking = false
            };

            // Act
            var result = _discountService.CheckDiscount(request, 500m);

            // Assert
            Assert.True(result.IsEligible);
            Assert.Equal(5, result.DiscountPercent);
        }

        [Fact]
        public void CheckDiscount_FiveContainers_ReturnsFivePercent()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 5,
                RequiresQuarantine = false,
                RequiresPacking = false,
                RequiresUnpacking = false
            };

            // Act
            var result = _discountService.CheckDiscount(request, 500m);

            // Assert
            Assert.True(result.IsEligible);
            Assert.Equal(5, result.DiscountPercent);
        }

        [Fact]
        public void CheckDiscount_HighValueJob_ReturnsEightPercent()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 1,
                RequiresQuarantine = false,
                RequiresPacking = false,
                RequiresUnpacking = false
            };

            // Act
            var result = _discountService.CheckDiscount(request, 1500m);

            // Assert
            Assert.True(result.IsEligible);
            Assert.Equal(8, result.DiscountPercent);
        }

        [Fact]
        public void CheckDiscount_QuarantineAndPacking_ReturnsTenPercent()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 1,
                RequiresQuarantine = true,
                RequiresPacking = true,
                RequiresUnpacking = false
            };

            // Act
            var result = _discountService.CheckDiscount(request, 500m);

            // Assert
            Assert.True(result.IsEligible);
            Assert.Equal(10, result.DiscountPercent);
        }

        [Fact]
        public void CheckDiscount_QuarantineAndUnpacking_ReturnsTenPercent()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 1,
                RequiresQuarantine = true,
                RequiresPacking = false,
                RequiresUnpacking = true
            };

            // Act
            var result = _discountService.CheckDiscount(request, 500m);

            // Assert
            Assert.True(result.IsEligible);
            Assert.Equal(10, result.DiscountPercent);
        }

        [Fact]
        public void CheckDiscount_NoEligibilityCriteriaMet_ReturnsNotEligible()
        {
            // Arrange
            var request = new QuotationRequest
            {
                NumberOfContainers = 1,
                RequiresQuarantine = false,
                RequiresPacking = false,
                RequiresUnpacking = false
            };

            // Act
            var result = _discountService.CheckDiscount(request, 500m);

            // Assert
            Assert.False(result.IsEligible);
            Assert.Equal(0, result.DiscountPercent);
        }

        // ── DISCOUNT CALCULATION TESTS ──

        [Fact]
        public void ApplyDiscount_FivePercent_CorrectlyReducesAmount()
        {
            // Arrange
            decimal subtotal = 1000m;

            // Act
            decimal result = _discountService.ApplyDiscount(subtotal, 5);

            // Assert
            Assert.Equal(950m, result);
        }

        [Fact]
        public void ApplyDiscount_TenPercent_CorrectlyReducesAmount()
        {
            // Arrange
            decimal subtotal = 1000m;

            // Act
            decimal result = _discountService.ApplyDiscount(subtotal, 10);

            // Assert
            Assert.Equal(900m, result);
        }

        // ── GST TESTS ──

        [Fact]
        public void ApplyGST_AddsCorrectly()
        {
            // Arrange
            decimal amount = 1000m;

            // Act
            decimal result = _discountService.ApplyGST(amount);

            // Assert
            Assert.Equal(1100m, result);
        }

        [Fact]
        public void ApplyGST_AfterDiscount_CorrectTotal()
        {
            // Arrange — 1000 with 10% discount = 900, then GST = 990
            decimal afterDiscount = _discountService.ApplyDiscount(1000m, 10);

            // Act
            decimal total = _discountService.ApplyGST(afterDiscount);

            // Assert
            Assert.Equal(990m, total);
        }
    }
}