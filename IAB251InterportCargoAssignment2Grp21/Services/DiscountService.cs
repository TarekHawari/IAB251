using IAB251InterportCargoAssignment2Grp21.Models;

namespace IAB251InterportCargoAssignment2Grp21.Services
{
    public class DiscountResult
    {
        public bool IsEligible { get; set; }
        public int DiscountPercent { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class DiscountService
    {
        public DiscountResult CheckDiscount(QuotationRequest request, decimal subtotal)
        {
            // Criteria 1: 3+ containers → 5%
            if (request.NumberOfContainers >= 3)
            {
                return new DiscountResult
                {
                    IsEligible = true,
                    DiscountPercent = 5,
                    Reason = $"{request.NumberOfContainers} containers requested — volume discount eligible (5%)."
                };
            }

            // Criteria 2: High value job → 8%
            if (subtotal > 1000m)
            {
                return new DiscountResult
                {
                    IsEligible = true,
                    DiscountPercent = 8,
                    Reason = $"High-value job (${subtotal:F2}) — competitive pricing discount eligible (8%)."
                };
            }

            // Criteria 3: Quarantine + packing/unpacking → 10%
            if (request.RequiresQuarantine && (request.RequiresPacking || request.RequiresUnpacking))
            {
                return new DiscountResult
                {
                    IsEligible = true,
                    DiscountPercent = 10,
                    Reason = "Combined quarantine and packing/unpacking services — discount eligible (10%)."
                };
            }

            return new DiscountResult { IsEligible = false };
        }

        public decimal ApplyDiscount(decimal subtotal, int discountPercent)
        {
            decimal discountAmount = Math.Round(subtotal * (discountPercent / 100m), 2);
            return subtotal - discountAmount;
        }

        public decimal ApplyGST(decimal amount)
        {
            return Math.Round(amount * 1.10m, 2);
        }
    }
}