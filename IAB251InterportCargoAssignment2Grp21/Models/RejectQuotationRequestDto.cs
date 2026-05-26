using System.ComponentModel.DataAnnotations;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class RejectQuotationRequestDto
    {
        [Required(ErrorMessage = "A rejection reason is required.")]
        public string RejectionMessage { get; set; } = string.Empty;
    }
}
