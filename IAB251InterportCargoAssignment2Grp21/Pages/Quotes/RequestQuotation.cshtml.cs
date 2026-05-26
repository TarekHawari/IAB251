using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class RequestQuotationModel : PageModel
    {
        private readonly QuotationServiceClient _quotationServiceClient;

        public RequestQuotationModel(QuotationServiceClient quotationServiceClient)
        {
            _quotationServiceClient = quotationServiceClient;
        }

        [BindProperty]
        public CreateQuotationRequestDto Request { get; set; } = new();

        public string SuccessMessage { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToPage("/Account/CustomerLogin");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            var customerName = HttpContext.Session.GetString("CustomerName");
            var customerEmail = HttpContext.Session.GetString("CustomerEmail");

            if (customerId == null || string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(customerEmail))
            {
                return RedirectToPage("/Account/CustomerLogin");
            }

            Request.CustomerId = customerId.Value;
            Request.CustomerName = customerName;
            Request.CustomerEmail = customerEmail;

            if (Request.RequiresQuarantine && string.IsNullOrWhiteSpace(Request.QuarantineDetails))
            {
                ModelState.AddModelError("Request.QuarantineDetails", "Quarantine details are required when quarantine is selected.");
            }

            if (Request.RequiresFumigation && string.IsNullOrWhiteSpace(Request.FumigationDetails))
            {
                ModelState.AddModelError("Request.FumigationDetails", "Fumigation details are required when fumigation is selected.");
            }


            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _quotationServiceClient.CreateAsync(Request);

            SuccessMessage = "Your quotation request has been submitted successfully.";
            ModelState.Clear();
            Request = new CreateQuotationRequestDto();

            return Page();
        }
    }
}