using InterportCargo.QuotationService.Data;
using InterportCargo.QuotationService.Models;
using InterportCargo.QuotationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.QuotationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationRequestsController : ControllerBase
    {
        private readonly QuotationDbContext _context;
        private readonly NotificationServiceClient _notificationServiceClient;

        public QuotationRequestsController(
            QuotationDbContext context,
            NotificationServiceClient notificationServiceClient)
        {
            _context = context;
            _notificationServiceClient = notificationServiceClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<QuotationRequest>>> GetAll()
        {
            var requests = await _context.QuotationRequests
                .OrderByDescending(q => q.SubmittedAt)
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<List<QuotationRequest>>> GetPending()
        {
            var requests = await _context.QuotationRequests
                .Where(q => q.Status == "Pending")
                .OrderByDescending(q => q.SubmittedAt)
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<List<QuotationRequest>>> GetByCustomerId(int customerId)
        {
            var requests = await _context.QuotationRequests
                .Where(q => q.CustomerId == customerId)
                .OrderByDescending(q => q.SubmittedAt)
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("{requestId:int}")]
        public async Task<ActionResult<QuotationRequest>> GetById(int requestId)
        {
            var request = await _context.QuotationRequests.FindAsync(requestId);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<QuotationRequest>> Create(CreateQuotationRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.RequiresQuarantine && string.IsNullOrWhiteSpace(dto.QuarantineDetails))
            {
                ModelState.AddModelError(nameof(dto.QuarantineDetails), "Quarantine details are required when quarantine is selected.");
            }

            if (dto.RequiresFumigation && string.IsNullOrWhiteSpace(dto.FumigationDetails))
            {
                ModelState.AddModelError(nameof(dto.FumigationDetails), "Fumigation details are required when fumigation is selected.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quotationRequest = new QuotationRequest
            {
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                Source = dto.Source,
                Destination = dto.Destination,
                NumberOfContainers = dto.NumberOfContainers,
                GoodsType = dto.GoodsType,
                PackageWidth = dto.PackageWidth,
                PackageHeight = dto.PackageHeight,
                ImportExportType = dto.ImportExportType,
                PackingType = dto.PackingType,
                RequiresQuarantine = dto.RequiresQuarantine,
                QuarantineDetails = dto.RequiresQuarantine ? dto.QuarantineDetails : null,
                RequiresFumigation = dto.RequiresFumigation,
                FumigationDetails = dto.RequiresFumigation ? dto.FumigationDetails : null,
                Status = "Pending",
                SubmittedAt = DateTime.Now
            };

            _context.QuotationRequests.Add(quotationRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { requestId = quotationRequest.RequestId }, quotationRequest);
        }

        [HttpPut("{requestId:int}/accept")]
        public async Task<IActionResult> Accept(int requestId)
        {
            var request = await _context.QuotationRequests.FindAsync(requestId);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Accepted";
            request.ReviewedAt = DateTime.Now;
            request.OfficerMessage = "Quotation request accepted for preparation.";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{requestId:int}/reject")]
        public async Task<IActionResult> Reject(int requestId, RejectQuotationRequestDto dto)
        {
            var request = await _context.QuotationRequests.FindAsync(requestId);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Rejected";
            request.ReviewedAt = DateTime.Now;
            request.OfficerMessage = dto.RejectionMessage;

            await _context.SaveChangesAsync();

            var notification = new CreateNotificationRequest
            {
                RecipientType = "Customer",
                CustomerId = request.CustomerId,
                QuotationRequestId = request.RequestId,
                Message = $"Your quotation request #{request.RequestId} from {request.Source} to {request.Destination} was rejected. Reason: {dto.RejectionMessage}"
            };

            await _notificationServiceClient.SendNotificationAsync(notification);

            return NoContent();
        }
    }
}