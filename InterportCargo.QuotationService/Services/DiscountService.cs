using InterportCargo.QuotationService.Models;

namespace InterportCargo.QuotationService.Services
{
    public class DiscountService
    {
        public decimal GetEligibleDiscountPercentage(QuotationRequest request)
        {
            if (request.NumberOfContainers > 10
                && request.RequiresQuarantine
                && request.RequiresFumigation)
            {
                return 10m;
            }

            if (request.NumberOfContainers > 5
                && request.RequiresQuarantine
                && request.RequiresFumigation)
            {
                return 5m;
            }

            if (request.NumberOfContainers > 5
                && (request.RequiresQuarantine || request.RequiresFumigation))
            {
                return 2.5m;
            }

            return 0m;
        }
    }
}
