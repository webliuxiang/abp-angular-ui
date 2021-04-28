using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Friendships.Dtos
{
    public class AcceptFriendshipRequestInput
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        public int? TenantId { get; set; }
    }
}
