using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IAB251InterportCargoAssignment2Grp21.Models;
using System.Net.Mail;
using IAB251InterportCargoAssignment2Grp21.Data;
using System.Reflection;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Account
{
    public class CustomerRegisterModel : PageModel
    {
        [BindProperty]
        public Customer customer { get; set; } = default!;

        private InterportCargoContext dbContext;

        public CustomerRegisterModel(InterportCargoContext _dbContext) {
            dbContext = _dbContext;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostCustomerRegister()   
        {
            if (!ModelState.IsValid)
                return Page();
            
            if (!validateForm())
                return Page();

            customer.Password = handlePassword(customer.Password);
            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();

            return RedirectToPage("/Account/CustomerLogin");
        }


        private bool validateForm() {
            if (validateEmail(customer.Email)) {
                ModelState.AddModelError("customer.Email", "Email already exists");
                return false;
            }
            return true;
        }

        private bool validateEmail(string email) {
            return dbContext.Customers.Any(user => user.Email == email); 
        }

        private string handlePassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
