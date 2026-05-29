namespace InterportCargo.QuotationService.Models
{
    public class RateSchedule
    {
        public decimal WharfBookingFee { get; set; }
        public decimal LiftOnLiftOffFee { get; set; }
        public decimal FumigationFee { get; set; }
        public decimal LclDeliveryDepotFee { get; set; }
        public decimal TailgateInspectionFee { get; set; }
        public decimal StorageFee { get; set; }
        public decimal FacilityFee { get; set; }
        public decimal WharfInspectionFee { get; set; }
        public decimal GstRate { get; set; }
    }
}
