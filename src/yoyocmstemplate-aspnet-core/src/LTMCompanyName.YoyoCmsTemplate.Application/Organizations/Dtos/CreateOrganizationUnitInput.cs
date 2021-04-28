using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    public class CreateOrganizationUnitInput
    {
        public long? ParentId { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
    }
}