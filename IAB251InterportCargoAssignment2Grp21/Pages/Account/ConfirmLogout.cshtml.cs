using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Account
{
    public class ConfirmLogoutModel : PageModel
    {
        public string EmployeeName { get; set; } = string.Empty;

        // Gets employee name and checks for logged in user session
        public IActionResult OnGet()
        {
            EmployeeName = HttpContext.Session.GetString("EmployeeName") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        // Logged in user session clear. Redirect to home page after logout.
        public IActionResult OnPost()
        {
            HttpContext.Session.Clear();

            return RedirectToPage("/Index");
        }
    }
}
