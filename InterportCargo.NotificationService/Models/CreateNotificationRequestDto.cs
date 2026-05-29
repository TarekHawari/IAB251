using System.ComponentModel.DataAnnotations;

namespace InterportCargo.NotificationService.Models
{
    public class CreateNotificationRequestDto
    {
        [Required]
        public string RecipientType { get; set; } = "Customer";

        public int? CustomerId { get; set; }

        public string? EmployeeEmail { get; set; }

        public int? QuotationRequestId { get; set; }

        public int? QuotationId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;
    }
}
