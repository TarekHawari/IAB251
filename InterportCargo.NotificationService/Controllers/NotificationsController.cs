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
                .Where(n => n.CustomerId == customerId)
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

            var notification = new CustomerNotification
            {
                CustomerId = request.CustomerId,
                QuotationRequestId = request.QuotationRequestId,
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
