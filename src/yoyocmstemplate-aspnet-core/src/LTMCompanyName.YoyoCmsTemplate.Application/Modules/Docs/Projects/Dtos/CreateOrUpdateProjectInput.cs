using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos
{
    public class CreateOrUpdateProjectInput
    {
        [Required]
        public ProjectEditDto Project { get; set; }

    }
}
