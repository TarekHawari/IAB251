using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class OfficerDashboardModel : PageModel
    {
        private readonly QuotationServiceClient _quotationServiceClient;

        public OfficerDashboardModel(QuotationServiceClient quotationServiceClient)
        {
            _quotationServiceClient = quotationServiceClient;
        }

        public string EmployeeName { get; set; } = string.Empty;

        public List<QuotationRequestDto> PendingRequests { get; set; } = new();

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

            PendingRequests = await _quotationServiceClient.GetPendingAsync();

            return Page();
        }
    }
}