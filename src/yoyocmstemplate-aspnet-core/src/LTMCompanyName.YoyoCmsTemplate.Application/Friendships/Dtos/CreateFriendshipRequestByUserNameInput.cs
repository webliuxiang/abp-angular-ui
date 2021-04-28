using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Friendships.Dtos
{
    public class CreateFriendshipRequestByUserNameInput
    {
        [Required(AllowEmptyStrings = true)]
        public string TenancyName { get; set; }

        public string UserName { get; set; }
    }
}
