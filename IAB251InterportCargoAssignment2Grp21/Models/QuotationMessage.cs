using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class QuotationMessage
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        public QuotationRequest? QuotationRequest { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        // "Officer" or "Customer"
        [StringLength(20)]
        public string SentBy { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}