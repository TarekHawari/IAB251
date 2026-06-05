using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class Quotation
    {
        [Key]
        public int QuotationId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        public QuotationRequest? QuotationRequest { get; set; }

        [StringLength(20)]
        public string QuotationNumber { get; set; } = string.Empty;

        public DateTime DateIssued { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string ContainerType { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Scope { get; set; } = string.Empty;

        // Charges
        [Column(TypeName = "decimal(10,2)")]
        public decimal BaseRate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal DepotCharges { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal LclDeliveryCharges { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal AdditionalCharges { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public bool DiscountApplied { get; set; }

        [StringLength(500)]
        public string? DiscountReason { get; set; }

        // Pending / Accepted / Rejected
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [StringLength(500)]
        public string? OfficerEmail { get; set; }
    }
}