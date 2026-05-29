using InterportCargo.QuotationService.Data;
using InterportCargo.QuotationService.Models;
using InterportCargo.QuotationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.QuotationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationsController : ControllerBase
    {
        private readonly QuotationDbContext _context;
        private readonly RateScheduleService _rateScheduleService;
        private readonly DiscountService _discountService;
        private readonly NotificationServiceClient _notificationServiceClient;

        public QuotationsController(
            QuotationDbContext context,
            RateScheduleService rateScheduleService,
            DiscountService discountService,
            NotificationServiceClient notificationServiceClient)
        {
            _context = context;
            _rateScheduleService = rateScheduleService;
            _discountService = discountService;
            _notificationServiceClient = notificationServiceClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<Quotation>>> GetAll()
        {
            var quotations = await _context.Quotations
                .OrderByDescending(q => q.DateIssued)
                .ToListAsync();

            return Ok(quotations);
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<List<Quotation>>> GetByCustomerId(int customerId)
        {
            var quotations = await _context.Quotations
                .Where(q => q.CustomerId == customerId)
                .OrderByDescending(q => q.DateIssued)
                .ToListAsync();

            return Ok(quotations);
        }

        [HttpGet("{quotationId:int}")]
        public async Task<ActionResult<Quotation>> GetById(int quotationId)
        {
            var quotation = await _context.Quotations.FindAsync(quotationId);

            if (quotation == null)
            {
                return NotFound();
            }

            return Ok(quotation);
        }

        [HttpPost("prepare/{quotationRequestId:int}")]
        public async Task<ActionResult<Quotation>> PrepareQuotation(
            int quotationRequestId,
            PrepareQuotationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = await _context.QuotationRequests.FindAsync(quotationRequestId);

            if (request == null)
            {
                return NotFound("Quotation request not found.");
            }

            if (request.Status == "Rejected")
            {
                return BadRequest("Cannot prepare a quotation for a rejected request.");
            }

            var existingQuotation = await _context.Quotations
                .FirstOrDefaultAsync(q => q.QuotationRequestId == quotationRequestId);

            if (existingQuotation != null)
            {
                return BadRequest("A quotation has already been prepared for this request.");
            }

            var rates = _rateScheduleService.GetRates(dto.ContainerType);
            var containerCount = request.NumberOfContainers;

            var wharfBookingFee = rates.WharfBookingFee * containerCount;
            var liftOnLiftOffFee = rates.LiftOnLiftOffFee * containerCount;
            var fumigationFee = request.RequiresFumigation ? rates.FumigationFee * containerCount : 0;
            var lclDeliveryDepotFee = rates.LclDeliveryDepotFee * containerCount;
            var tailgateInspectionFee = request.RequiresQuarantine ? rates.TailgateInspectionFee * containerCount : 0;
            var storageFee = rates.StorageFee * containerCount;
            var facilityFee = rates.FacilityFee * containerCount;
            var wharfInspectionFee = rates.WharfInspectionFee * containerCount;

            var subtotalBeforeDiscount =
                wharfBookingFee +
                liftOnLiftOffFee +
                fumigationFee +
                lclDeliveryDepotFee +
                tailgateInspectionFee +
                storageFee +
                facilityFee +
                wharfInspectionFee;

            var eligibleDiscountPercentage = _discountService.GetEligibleDiscountPercentage(request);
            var discountEligible = eligibleDiscountPercentage > 0;

            var discountPercentage = dto.ApplyDiscount && discountEligible
                ? eligibleDiscountPercentage
                : 0;

            var discountAmount = subtotalBeforeDiscount * (discountPercentage / 100m);
            var subtotalAfterDiscount = subtotalBeforeDiscount - discountAmount;

            var gstAmount = subtotalAfterDiscount * rates.GstRate;
            var totalAmount = subtotalAfterDiscount + gstAmount;

            var quotation = new Quotation
            {
                QuotationRequestId = request.RequestId,
                QuotationNumber = GenerateQuotationNumber(),
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                DateIssued = DateTime.Now,
                ContainerType = dto.ContainerType,
                ScopeDescription = dto.ScopeDescription,

                WharfBookingFee = wharfBookingFee,
                LiftOnLiftOffFee = liftOnLiftOffFee,
                FumigationFee = fumigationFee,
                LclDeliveryDepotFee = lclDeliveryDepotFee,
                TailgateInspectionFee = tailgateInspectionFee,
                StorageFee = storageFee,
                FacilityFee = facilityFee,
                WharfInspectionFee = wharfInspectionFee,

                SubtotalBeforeDiscount = subtotalBeforeDiscount,
                DiscountEligible = discountEligible,
                DiscountPercentage = discountPercentage,
                DiscountApplied = discountPercentage > 0,
                DiscountAmount = discountAmount,
                GstAmount = gstAmount,
                TotalAmount = totalAmount,
                Status = "Pending"
            };

            _context.Quotations.Add(quotation);

            request.Status = "QuotationPrepared";
            request.ReviewedAt = DateTime.Now;
            request.OfficerMessage = "Quotation prepared and sent to customer.";

            await _context.SaveChangesAsync();

            await _notificationServiceClient.SendNotificationAsync(new CreateNotificationRequest
            {
                RecipientType = "Customer",
                CustomerId = request.CustomerId,
                QuotationRequestId = request.RequestId,
                QuotationId = quotation.QuotationId,
                Message = $"A quotation has been prepared for your request #{request.RequestId}. Please review quotation {quotation.QuotationNumber}."
            });

            return CreatedAtAction(nameof(GetById), new { quotationId = quotation.QuotationId }, quotation);
        }

        [HttpPut("{quotationId:int}/customer-accept")]
        public async Task<IActionResult> CustomerAccept(int quotationId)
        {
            var quotation = await _context.Quotations.FindAsync(quotationId);

            if (quotation == null)
            {
                return NotFound();
            }

            quotation.Status = "Accepted";
            await _context.SaveChangesAsync();

            await _notificationServiceClient.SendNotificationAsync(new CreateNotificationRequest
            {
                RecipientType = "Officer",
                EmployeeEmail = "t.williams@company.com",
                QuotationId = quotation.QuotationId,
                QuotationRequestId = quotation.QuotationRequestId,
                Message = $"Customer {quotation.CustomerName} accepted quotation {quotation.QuotationNumber}."
            });

            return NoContent();
        }

        [HttpPut("{quotationId:int}/customer-reject")]
        public async Task<IActionResult> CustomerReject(int quotationId)
        {
            var quotation = await _context.Quotations.FindAsync(quotationId);

            if (quotation == null)
            {
                return NotFound();
            }

            quotation.Status = "Rejected";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static string GenerateQuotationNumber()
        {
            return $"Q-{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}