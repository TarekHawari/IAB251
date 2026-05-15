using System.ComponentModel.DataAnnotations;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        // VALIDATION XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Family name is required.")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters.")]
        public string FamilyName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(20, ErrorMessage = "Cannot exceed 20 characters.")] 
        // [] 
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(150, ErrorMessage = "Cannot exceed 150 characters.")] 
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Cannot exceed 250 characters.")] 
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters.")] 
        public string Password { get; set; } = string.Empty;
    }
}
