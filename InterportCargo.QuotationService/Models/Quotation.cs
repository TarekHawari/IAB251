using System.ComponentModel.DataAnnotations;

namespace InterportCargo.QuotationService.Models
{
    public class Quotation
    {
        [Key]
        public int QuotationId { get; set; }

        [Required]
        public int QuotationRequestId { get; set; }

        [Required]
        [StringLength(30)]
        public string QuotationNumber { get; set; } = string.Empty;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        public DateTime DateIssued { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string ContainerType { get; set; } = string.Empty;
        // 20 Feet or 40 Feet

        [Required]
        [StringLength(500)]
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

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        // Pending, Accepted, Rejected, Closed
    }
}