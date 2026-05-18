using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var employeeRole = HttpContext.Session.GetString("EmployeeRole");

            if (!string.IsNullOrWhiteSpace(employeeRole))
            {
                var normalisedRole = employeeRole.Replace(" ", "").Trim().ToLowerInvariant();

                if (normalisedRole == "quotationofficer")
                {
                    return RedirectToPage("/Quotes/OfficerDashboard");
                }
            }

            return Page();
        }
    }
}
