namespace InterportCargo.QuotationService.Models
{
    public class CreateNotificationRequest
    {
        public string RecipientType { get; set; } = "Customer";

        public int? CustomerId { get; set; }

        public string? EmployeeEmail { get; set; }

        public int? QuotationRequestId { get; set; }

        public int? QuotationId { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}