using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class ViewQuotationModel : PageModel
    {
        private readonly QuotationServiceClient _quotationServiceClient;

        public ViewQuotationModel(QuotationServiceClient quotationServiceClient)
        {
            _quotationServiceClient = quotationServiceClient;
        }

        [BindProperty(SupportsGet = true)]
        public int QuotationId { get; set; }

        public QuotationDto? Quotation { get; set; }
        
        public bool isCustomer {get; set;}

        public async Task<IActionResult> OnGetAsync()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            var employeeRole = HttpContext.Session.GetString("EmployeeRole");

            isCustomer = customerId != null;

            if (customerId == null && employeeRole == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Quotation = await _quotationServiceClient.GetQuotationByIdAsync(QuotationId);

            if (Quotation == null)
            {
                if (employeeRole != null )
                    return RedirectToPage("/Quotes/OfficerDashboard");
                    
                return RedirectToPage("/Quotes/CustomerDashboard");
            }
            
            if (customerId != null && Quotation.CustomerId != customerId.Value)
            {
                return RedirectToPage("/Quotes/CustomerDashboard");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptAsync()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToPage("/Account/CustomerLogin");
            }

            await _quotationServiceClient.CustomerAcceptQuotationAsync(QuotationId);

            return RedirectToPage("/Quotes/CustomerDashboard");
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToPage("/Account/CustomerLogin");
            }

            await _quotationServiceClient.CustomerRejectQuotationAsync(QuotationId);

            return RedirectToPage("/Quotes/CustomerDashboard");
        }
    }
}