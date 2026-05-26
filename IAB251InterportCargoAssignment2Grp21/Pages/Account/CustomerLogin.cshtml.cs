using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IAB251InterportCargoAssignment2Grp21.Models;
using System.Net.Mail;
using IAB251InterportCargoAssignment2Grp21.Data;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Account
{
    public class CustomerLoginModel : PageModel
    {
        [BindProperty]
        public Customer customer { get; set; } = default!;

        private InterportCargoContext dbContext;
        
        public CustomerLoginModel(InterportCargoContext _dbContext) {
            dbContext = _dbContext;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPostCustomerLogin()   
        {
            if (!validateForm())
                return Page();

            return RedirectToPage("/Quotes/CustomerDashboard");
        }


        private bool validateForm() {  
            bool notValid = false;
            
            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                ModelState.AddModelError("customer.Email", "Please enter your email.");
                notValid = true;
            }

            if (string.IsNullOrWhiteSpace(customer.Password))
            {
                ModelState.AddModelError("customer.Password", "Please enter your password.");
                notValid = true;
            }

            if (notValid)
                return false;


            if (!validateEmail(customer.Email)) {
                ModelState.AddModelError("customer.Email", "Email does not exist");
                return false;
            }

            if (!verifyPassword())
                return false;        

            return true;
        }

        private bool validateEmail(string email) {
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

        private bool verifyPassword() {
            Customer? user = dbContext.Customers.FirstOrDefault(user => user.Email == customer.Email);
            
            if (user == null) {
                ModelState.AddModelError("customer.Email", "Cannot login, try again later");
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(customer.Password, user.Password)) {
                ModelState.AddModelError("customer.Password", "Invalid Password");
                return false;
            }

            createSession(user);
            return true;
        }

        private void createSession(Customer user) {
            string fullname = $"{user.FirstName} {user.FamilyName}";

            HttpContext.Session.SetInt32("CustomerId", user.CustomerId);
            HttpContext.Session.SetString("CustomerEmail", user.Email);
            HttpContext.Session.SetString("CustomerName", fullname);


            
        }
        
    }

    
}
