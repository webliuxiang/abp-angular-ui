using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.Dtos
{
    public class CreateOrUpdateMemberInput
    {
        [Required]
        public MemberEditDto Member { get; set; }

    }
}
