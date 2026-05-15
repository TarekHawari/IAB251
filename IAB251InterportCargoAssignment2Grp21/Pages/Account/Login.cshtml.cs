using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.EntityFrameworkCore;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string? email { get; set; }
        
        [BindProperty]
        public string? password { get; set; }
        

        public void OnGet()
        {
            
        }

        public IActionResult OnPostCustomerLogin()   
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("customer");
            Console.WriteLine($"This is email: {email}");
            Console.WriteLine(password);
            Console.WriteLine("-------------------------------------------------");
            return Page();
        }

        public IActionResult OnPostEmployeeLogin()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("employee");
            Console.WriteLine($"This is employee: {email}");
            Console.WriteLine(password);
            Console.WriteLine("-------------------------------------------------");
            return Page();
        }
    }

    
}
