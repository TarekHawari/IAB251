using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class PrepareQuotationModel : PageModel
    {
        private readonly QuotationServiceClient _quotationServiceClient;

        public PrepareQuotationModel(QuotationServiceClient quotationServiceClient)
        {
            _quotationServiceClient = quotationServiceClient;
        }

        [BindProperty(SupportsGet = true)]
        public int RequestId { get; set; }

        public QuotationRequestDto? QuotationRequest { get; set; }

        [BindProperty]
        public PrepareQuotationDto Quotation { get; set; } = new();

        public decimal EligibleDiscountPercentage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsQuotationOfficer())
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            QuotationRequest = await _quotationServiceClient.GetByIdAsync(RequestId);

            if (QuotationRequest == null)
            {
                return Page();
            }

            EligibleDiscountPercentage = CalculateEligibleDiscount(QuotationRequest);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsQuotationOfficer())
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            QuotationRequest = await _quotationServiceClient.GetByIdAsync(RequestId);

            if (QuotationRequest == null)
            {
                return Page();
            }

            EligibleDiscountPercentage = CalculateEligibleDiscount(QuotationRequest);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var employeeEmail = HttpContext.Session.GetString("EmployeeEmail");

            if (string.IsNullOrWhiteSpace(employeeEmail))
            {
                return RedirectToPage("/Account/EmployeeLogin");
            }

            Quotation.PreparedByEmployeeEmail = employeeEmail;

            await _quotationServiceClient.PrepareQuotationAsync(RequestId, Quotation);

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

        private static decimal CalculateEligibleDiscount(QuotationRequestDto request)
        {
            if (request.NumberOfContainers > 10
                && request.RequiresQuarantine
                && request.RequiresFumigation)
            {
                return 10m;
            }

            if (request.NumberOfContainers > 5
                && request.RequiresQuarantine
                && request.RequiresFumigation)
            {
                return 5m;
            }

            if (request.NumberOfContainers > 5
                && (request.RequiresQuarantine || request.RequiresFumigation))
            {
                return 2.5m;
            }

            return 0m;
        }
    }
}