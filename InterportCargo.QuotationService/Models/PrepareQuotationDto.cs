using System.ComponentModel.DataAnnotations;

namespace InterportCargo.QuotationService.Models
{
    public class PrepareQuotationDto
    {
        [Required]
        [StringLength(20)]
        public string ContainerType { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ScopeDescription { get; set; } = string.Empty;

        public bool ApplyDiscount { get; set; }

        [Required]
        [StringLength(100)]
        public string PreparedByEmployeeEmail { get; set; } = string.Empty;
    }
}
