using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos
{
    public class CreateOrUpdateVideoResourceInput
    {
        [Required]
        public VideoResourceEditDto VideoResource { get; set; }

    }
}
