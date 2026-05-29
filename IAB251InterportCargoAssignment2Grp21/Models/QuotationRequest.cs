using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class QuotationRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Required(ErrorMessage = "Source is required.")]
        [StringLength(100)]
        public string Source { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination is required.")]
        [StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Number of containers is required.")]
        [Range(1, 100, ErrorMessage = "Must be between 1 and 100.")]
        public int NumberOfContainers { get; set; }

        [Required(ErrorMessage = "Package nature is required.")]
        [StringLength(200)]
        public string PackageNature { get; set; } = string.Empty;

        // Nature of Job fields
        public bool IsImport { get; set; }
        public bool IsExport { get; set; }
        public bool RequiresPacking { get; set; }
        public bool RequiresUnpacking { get; set; }
        public bool RequiresQuarantine { get; set; }

        [StringLength(500)]
        public string? AdditionalNotes { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Pending / Accepted / Rejected
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public Quotation? Quotation { get; set; }
        public ICollection<QuotationMessage> Messages { get; set; } = new List<QuotationMessage>();
    }
}