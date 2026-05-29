using System.ComponentModel.DataAnnotations;

namespace InterportCargo.QuotationService.Models
{
    public class CreateQuotationRequestDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public string Source { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int NumberOfContainers { get; set; }

        [Required]
        public string GoodsType { get; set; } = string.Empty;

        [Range(0.01, 10000)]
        public decimal PackageWidth { get; set; }

        [Range(0.01, 10000)]
        public decimal PackageHeight { get; set; }

        [Required]
        public string ImportExportType { get; set; } = string.Empty;

        [Required]
        public string PackingType { get; set; } = string.Empty;

        public bool RequiresQuarantine { get; set; }

        public string? QuarantineDetails { get; set; }

        public bool RequiresFumigation { get; set; }

        public string? FumigationDetails { get; set; }
    }
}
