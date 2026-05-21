using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class CustomerDashboardModel : PageModel
    {
        // public string CName { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var CustomerName = HttpContext.Session.GetString("CustomerName");

            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                return RedirectToPage("/Account/CustomerLogin");
            }

            return Page();
        }
    }
}
