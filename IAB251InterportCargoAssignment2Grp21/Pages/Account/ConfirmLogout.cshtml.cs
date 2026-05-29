using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Account
{
    public class ConfirmLogoutModel : PageModel
    {
        public string DisplayName { get; set; } = string.Empty;
        public string ReturnPage { get; set; } = "/Index";
        public string PortalName { get; set; } = "InterportCargo portal";

        // Gets employee name and checks for logged in user session
        public IActionResult OnGet()
        {

            var employeeName = HttpContext.Session.GetString("EmployeeName");
            var customerName = HttpContext.Session.GetString("CustomerName");
  

            if (!string.IsNullOrWhiteSpace(employeeName))
            {
                DisplayName = employeeName;
                ReturnPage = "/Quotes/OfficerDashboard";
                PortalName = "quotation officer portal";
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                DisplayName = customerName;
                ReturnPage = "/Quotes/CustomerDashboard";
                PortalName = "customer portal";
                return Page();
            }

            return RedirectToPage("/Index");
        }

        // Logged in user session clear. Redirect to home page after logout.
        public IActionResult OnPost()
        {
            HttpContext.Session.Clear();

            return RedirectToPage("/Index");
        }
    }
}
