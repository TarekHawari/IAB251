using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class CustomerDashboardModel : PageModel
    {
        public string CustomerFullName { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var CustomerFullName = HttpContext.Session.GetString("CustomerName");

            if (string.IsNullOrWhiteSpace(CustomerFullName))
            {
                return RedirectToPage("/Account/CustomerLogin");
            }

            return Page();
        }
    }
}
