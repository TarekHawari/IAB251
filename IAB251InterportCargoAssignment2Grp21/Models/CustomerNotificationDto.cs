namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class CustomerNotificationDto
    {
        public int CustomerNotificationId { get; set; }
        public int CustomerId { get; set; }
        public int QuotationRequestId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}