using System.ComponentModel.DataAnnotations;

namespace InterportCargo.QuotationService.Models
{
    public class RejectQuotationRequestDto
    {
        [Required]
        [StringLength(500)]
        public string RejectionMessage { get; set; } = string.Empty;
    }
}