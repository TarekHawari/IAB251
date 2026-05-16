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
        public Customer customer { get; set; }

        private InterportCargoContext dbContext;

        public CustomerRegisterModel(InterportCargoContext _dbContext) {
            dbContext = _dbContext;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostCustomerRegister()   
        {
            if (!validateForm())
                return Page();

            customer.Password = handlePassword(customer!.Password);
            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();

            return RedirectToPage("/Account/CustomerLogin");
        }


        private bool validateForm() {
            if (validateEmail(customer.Email)) {
                ModelState.AddModelError("customer.Email", "Email already exists");
                return false;
            }

            // each input field is put into an array then handeled to check for nulls and empty
            PropertyInfo[] inputs = customer.GetType().GetProperties();
            foreach (PropertyInfo input in inputs) {
                if (string.IsNullOrEmpty(input.GetValue(customer)?.ToString()))
                    return false;
            }
            return true;
        }

        private bool validateEmail(string? email) {
            try {
                // if invalid type, then it will be caught
                MailAddress validity = new MailAddress(email);
            
                // validates if the user put correct email
                if (validity.Address != email)
                    return false;
            }
            catch {
                return false;
            }
            return dbContext.Customers.Any(user => user.Email == email); 
        }

        private string handlePassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
