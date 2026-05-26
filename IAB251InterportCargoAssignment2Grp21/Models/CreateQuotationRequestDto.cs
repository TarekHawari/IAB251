using System.ComponentModel.DataAnnotations;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class CreateQuotationRequestDto
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Source is required.")]
        public string Source { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination is required.")]
        public string Destination { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Number of containers must be at least 1.")]
        public int NumberOfContainers { get; set; }




        [Required(ErrorMessage = "Goods type is required.")]
        public string GoodsType { get; set; } = string.Empty;

        [Range(0.01, 100000, ErrorMessage = "Package width must be entered in millimetres and must be greater than 0.")]
        public decimal PackageWidth { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Package height must be entered in millimetres and must be greater than 0.")]
        public decimal PackageHeight { get; set; }




        [Required(ErrorMessage = "Import/export type is required.")]
        public string ImportExportType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Packing type is required.")]
        public string PackingType { get; set; } = string.Empty;

        public bool RequiresQuarantine { get; set; }

        public string? QuarantineDetails { get; set; }

        public bool RequiresFumigation { get; set; }

        public string? FumigationDetails { get; set; }
    }
}
