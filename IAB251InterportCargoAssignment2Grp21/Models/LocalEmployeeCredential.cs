using System.ComponentModel.DataAnnotations;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class LocalEmployeeCredential
    {
        [Key]
        public int LocalEmployeeCredentialId { get; set; }

        [Required(ErrorMessage = "Employee email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee key is required.")]
        [StringLength(50, ErrorMessage = "Employee key cannot exceed 50 characters.")]
        public string EmployeeKey { get; set; } = string.Empty;
    }
}
