using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IAB251InterportCargoAssignment2Grp21.Models;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Customer? customer { get; set; }

        public RegisterModel() {

        }

        public void OnGet()
        {

        }

        public IActionResult OnPostCustomerRegister()   
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"{customer.Email}");
            // Console.WriteLine($"This is email: {email}");
            // Console.WriteLine(password);
            Console.WriteLine("-------------------------------------------------");
            return Page();
        }
    }
}
