using System.ComponentModel.DataAnnotations;

namespace InterportCargo.NotificationService.Models
{
    public class CustomerNotification
    {
        [Key]
        public int CustomerNotificationId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int QuotationRequestId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}