using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class OfficerQuotesModel : PageModel
    {
        private readonly QuotationServiceClient _quotationServiceClient;

        public OfficerQuotesModel(QuotationServiceClient quotationServiceClient)
        {
            _quotationServiceClient = quotationServiceClient;
        }
        
        
        public List<QuotationRequestDto> Requests { get; set; } = new();

        public List<QuotationDto> Quotations { get; set; } = new();


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

            Requests = await _quotationServiceClient.GetAllAsync();
            Quotations = await _quotationServiceClient.GetAllQuotationsAsync();

            return Page();
        }
    }
}