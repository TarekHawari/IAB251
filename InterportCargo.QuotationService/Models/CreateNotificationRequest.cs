namespace InterportCargo.QuotationService.Models
{
    public class CreateNotificationRequest
    {
        public int CustomerId { get; set; }
        public int QuotationRequestId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}