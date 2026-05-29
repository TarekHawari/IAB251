using InterportCargo.QuotationService.Models;

namespace InterportCargo.QuotationService.Services
{
    public class RateScheduleService
    {
        public RateSchedule GetRates(string containerType)
        {
            if (containerType == "20 Feet")
            {
                return new RateSchedule
                {
                    WharfBookingFee = 60,
                    LiftOnLiftOffFee = 80,
                    FumigationFee = 220,
                    LclDeliveryDepotFee = 400,
                    TailgateInspectionFee = 120,
                    StorageFee = 240,
                    FacilityFee = 70,
                    WharfInspectionFee = 60,
                    GstRate = 0.10m
                };
            }

            if (containerType == "40 Feet")
            {
                return new RateSchedule
                {
                    WharfBookingFee = 70,
                    LiftOnLiftOffFee = 120,
                    FumigationFee = 280,
                    LclDeliveryDepotFee = 500,
                    TailgateInspectionFee = 160,
                    StorageFee = 300,
                    FacilityFee = 100,
                    WharfInspectionFee = 90,
                    GstRate = 0.10m
                };
            }

            throw new ArgumentException("Invalid container type.");
        }
    }
}

//Future enhancement: Consider loading rates from a configuration file or database to allow for easier updates without code changes.