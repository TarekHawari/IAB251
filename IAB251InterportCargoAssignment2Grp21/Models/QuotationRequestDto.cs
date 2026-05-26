namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class QuotationRequestDto
    {
        public int RequestId { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int NumberOfContainers { get; set; }

        public string GoodsType { get; set; } = string.Empty;
        public decimal PackageWidth { get; set; }
        public decimal PackageHeight { get; set; }

        public string ImportExportType { get; set; } = string.Empty;
        public string PackingType { get; set; } = string.Empty;

        public bool RequiresQuarantine { get; set; }
        public string? QuarantineDetails { get; set; }

        public bool RequiresFumigation { get; set; }
        public string? FumigationDetails { get; set; }

        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? OfficerMessage { get; set; }
    }
}