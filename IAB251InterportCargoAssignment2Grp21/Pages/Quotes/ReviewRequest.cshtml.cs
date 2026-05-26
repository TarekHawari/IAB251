using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class ReviewRequestModel : PageModel
    {
        private readonly QuotationServiceClient _quotationServiceClient;

        public ReviewRequestModel(QuotationServiceClient quotationServiceClient)
        {
            _quotationServiceClient = quotationServiceClient;
        }

        [BindProperty(SupportsGet = true)]
        public int RequestId { get; set; }

        public QuotationRequestDto? QuotationRequest { get; set; }

        [BindProperty]
        public RejectQuotationRequestDto RejectDto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsQuotationOfficer())
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            QuotationRequest = await _quotationServiceClient.GetByIdAsync(RequestId);

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptAsync()
        {
            if (!IsQuotationOfficer())
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            await _quotationServiceClient.AcceptAsync(RequestId);

            return RedirectToPage("/Quotes/OfficerQuotes");
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            if (!IsQuotationOfficer())
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            if (!ModelState.IsValid)
            {
                QuotationRequest = await _quotationServiceClient.GetByIdAsync(RequestId);
                return Page();
            }

            await _quotationServiceClient.RejectAsync(RequestId, RejectDto);

            return RedirectToPage("/Quotes/OfficerQuotes");
        }

        private bool IsQuotationOfficer()
        {
            var employeeRole = HttpContext.Session.GetString("EmployeeRole");

            if (string.IsNullOrWhiteSpace(employeeRole))
            {
                return false;
            }

            return employeeRole.Replace(" ", "").Trim().ToLowerInvariant() == "quotationofficer";
        }
    }
}