using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos
{
    public class CreateOrUpdateDropdownListInput
    {
        [Required]
        public DropdownListEditDto DropdownList { get; set; }
							
    }
}
