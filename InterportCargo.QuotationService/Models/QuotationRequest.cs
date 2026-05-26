using System.ComponentModel.DataAnnotations;

namespace InterportCargo.QuotationService.Models
{
    public class QuotationRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Source { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string GoodsType { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000)]
        public decimal PackageWidth { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal PackageHeight { get; set; }

        [Required]
        [Range(1, 1000)]
        public int NumberOfContainers { get; set; }

        [Required]
        [StringLength(20)]
        public string ImportExportType { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PackingType { get; set; } = string.Empty;

        public bool RequiresQuarantine { get; set; }

        [StringLength(500)]
        public string? QuarantineDetails { get; set; }

        public bool RequiresFumigation { get; set; }

        [StringLength(500)]
        public string? FumigationDetails { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public DateTime? ReviewedAt { get; set; }

        [StringLength(500)]
        public string? OfficerMessage { get; set; }
    }
}
