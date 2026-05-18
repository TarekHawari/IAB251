using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class OfficerDashboardModel : PageModel
    {
        public string EmployeeName { get; set; } = string.Empty;

        public IActionResult OnGet()
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

            return Page();
        }
    }
}
