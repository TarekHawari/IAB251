using System.ComponentModel.DataAnnotations;

namespace IAB251InterportCargoAssignment2Grp21.Models
{
    public class PrepareQuotationDto
    {
        [Required(ErrorMessage = "Container type is required.")]
        public string ContainerType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Scope description is required.")]
        public string ScopeDescription { get; set; } = string.Empty;

        public bool ApplyDiscount { get; set; }

        public string PreparedByEmployeeEmail { get; set; } = string.Empty;
    }
}