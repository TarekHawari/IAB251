namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class CustomerNotificationDto
    {
        public int CustomerNotificationId { get; set; }

        public string RecipientType { get; set; } = string.Empty;

        public int? CustomerId { get; set; }

        public string? EmployeeEmail { get; set; }

        public int? QuotationRequestId { get; set; }

        public int? QuotationId { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}