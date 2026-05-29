using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class CustomerDashboardModel : PageModel
    {
        private readonly NotificationServiceClient _notificationServiceClient;
        private readonly QuotationServiceClient _quotationServiceClient;

        public CustomerDashboardModel(
            NotificationServiceClient notificationServiceClient,
            QuotationServiceClient quotationServiceClient)
        {
            _notificationServiceClient = notificationServiceClient;
            _quotationServiceClient = quotationServiceClient;
        }

        public string CustomerName { get; set; } = string.Empty;

        public List<CustomerNotificationDto> Notifications { get; set; } = new();

        public List<QuotationRequestDto> QuotationRequests { get; set; } = new();

        public List<QuotationDto> Quotations { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToPage("/Account/CustomerLogin");
            }

            CustomerName = HttpContext.Session.GetString("CustomerName") ?? "Customer";

            Notifications = await _notificationServiceClient.GetByCustomerIdAsync(customerId.Value);
            QuotationRequests = await _quotationServiceClient.GetByCustomerIdAsync(customerId.Value);
            Quotations = await _quotationServiceClient.GetQuotationsByCustomerIdAsync(customerId.Value);

            return Page();
        }
    }
}
