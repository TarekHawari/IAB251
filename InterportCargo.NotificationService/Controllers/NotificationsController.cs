using InterportCargo.NotificationService.Data;
using InterportCargo.NotificationService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationDbContext _context;

        public NotificationsController(NotificationDbContext context)
        {
            _context = context;
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<List<CustomerNotification>>> GetByCustomerId(int customerId)
        {
            var notifications = await _context.CustomerNotifications
                .Where(n => n.RecipientType == "Customer" && n.CustomerId == customerId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(notifications);
        }



        [HttpGet("officer/{employeeEmail}")]
        public async Task<ActionResult<List<CustomerNotification>>> GetByOfficerEmail(string employeeEmail)
        {
            var notifications = await _context.CustomerNotifications
                .Where(n => n.RecipientType == "Officer" && n.EmployeeEmail == employeeEmail)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(notifications);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (request.RecipientType == "Customer" && request.CustomerId == null)
            {
                return BadRequest("CustomerId is required for customer notifications.");
            }


            if (request.RecipientType == "Officer" && string.IsNullOrWhiteSpace(request.EmployeeEmail))
            {
                return BadRequest("EmployeeEmail is required for officer notifications.");
            }


            var notification = new CustomerNotification
            {
                RecipientType = request.RecipientType,
                CustomerId = request.CustomerId,
                EmployeeEmail = request.EmployeeEmail,
                QuotationRequestId = request.QuotationRequestId,
                QuotationId = request.QuotationId,
                Message = request.Message,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _context.CustomerNotifications.Add(notification);
            await _context.SaveChangesAsync();

            return Ok(notification);
        }

        [HttpPut("{notificationId:int}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var notification = await _context.CustomerNotifications.FindAsync(notificationId);

            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
