using System.ComponentModel.DataAnnotations;

namespace InterportCargo.NotificationService.Models
{
    public class CreateNotificationRequestDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int QuotationRequestId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;
    }
}
