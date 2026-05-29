using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class OfficerDashboardModel : PageModel
    {
        private readonly QuotationServiceClient _quotationServiceClient;
        private readonly NotificationServiceClient _notificationServiceClient;

        public OfficerDashboardModel(QuotationServiceClient quotationServiceClient, NotificationServiceClient notificationServiceClient)
        {
            _quotationServiceClient = quotationServiceClient;
            _notificationServiceClient = notificationServiceClient;
        }

        public string EmployeeName { get; set; } = string.Empty;

        public string EmployeeEmail { get; set; } = string.Empty;

        public List<QuotationRequestDto> PendingRequests { get; set; } = new();
        public List<CustomerNotificationDto> OfficerNotifications { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var employeeRole = HttpContext.Session.GetString("EmployeeRole");

            if (string.IsNullOrWhiteSpace(employeeRole))
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            var normalisedRole = employeeRole.Replace(" ", "").Trim().ToLowerInvariant();

            if (normalisedRole != "quotationofficer")
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            EmployeeName = HttpContext.Session.GetString("EmployeeName") ?? "Quotation Officer";
            EmployeeEmail = HttpContext.Session.GetString("EmployeeEmail") ?? "t.williams@company.com";


            PendingRequests = await _quotationServiceClient.GetPendingAsync();
            OfficerNotifications = await _notificationServiceClient.GetByOfficerEmailAsync(EmployeeEmail);

            return Page();
        }
    }
}