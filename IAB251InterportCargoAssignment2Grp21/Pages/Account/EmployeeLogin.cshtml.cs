using System.ComponentModel.DataAnnotations;
using IAB251InterportCargoAssignment2Grp21.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Account
{
    public class EmployeeLoginModel : PageModel
    {
        private readonly IEmployeeLoginAppService _employeeLoginAppService;

        public EmployeeLoginModel(IEmployeeLoginAppService employeeLoginAppService)
        {
            _employeeLoginAppService = employeeLoginAppService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Work Email")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Employee key is required.")]
        [Display(Name = "Employee Key")]
        public string EmployeeKey { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _employeeLoginAppService.LoginAsync(Email, EmployeeKey);

            if (!result.IsSuccessful)
            {
                ErrorMessage = "Login failed. Please check your employee login details and try again.";
                return Page();
            }

            HttpContext.Session.SetString("EmployeeEmail", result.EmployeeEmail);
            HttpContext.Session.SetString("EmployeeName", result.EmployeeName);
            HttpContext.Session.SetString("EmployeeRole", result.Role);

            return RedirectToPage("/Quotes/OfficerDashboard");
        }
    }
}
