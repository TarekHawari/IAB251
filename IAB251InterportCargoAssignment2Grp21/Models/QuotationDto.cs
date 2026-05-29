namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class QuotationDto
    {
        public int QuotationId { get; set; }
        public int QuotationRequestId { get; set; }

        public string QuotationNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public DateTime DateIssued { get; set; }

        public string ContainerType { get; set; } = string.Empty;

        public string ScopeDescription { get; set; } = string.Empty;

        public decimal WharfBookingFee { get; set; }
        public decimal LiftOnLiftOffFee { get; set; }
        public decimal FumigationFee { get; set; }
        public decimal LclDeliveryDepotFee { get; set; }
        public decimal TailgateInspectionFee { get; set; }
        public decimal StorageFee { get; set; }
        public decimal FacilityFee { get; set; }
        public decimal WharfInspectionFee { get; set; }

        public decimal SubtotalBeforeDiscount { get; set; }

        public bool DiscountEligible { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool DiscountApplied { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal GstAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}