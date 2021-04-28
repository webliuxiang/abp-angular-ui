using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace LTMCompanyName.YoyoCmsTemplate.Friendships
{
    [Table(AppConsts.TablePrefix + "Friendships")]
    public class Friendship : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        /// <summary>
        /// 数据所属人Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 所属租户Id
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 目标Id
        /// </summary>
        public long FriendUserId { get; set; }
        /// <summary>
        /// 目标租户Id
        /// </summary>
        public int? FriendTenantId { get; set; }
        /// <summary>
        /// 目标用户名
        /// </summary>
        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string FriendUserName { get; set; }
        /// <summary>
        /// 目标租户名称
        /// </summary>
        public string FriendTenancyName { get; set; }
        /// <summary>
        /// 头像Id
        /// </summary>
        public Guid? FriendProfilePictureId { get; set; }

        /// <summary>
        /// 拉黑状态  认可 / 不通
        /// </summary>
        public FriendshipState State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public Friendship(UserIdentifier user, UserIdentifier probableFriend, string probableFriendTenancyName, string probableFriendUserName, Guid? probableFriendProfilePictureId, FriendshipState state)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (probableFriend == null)
            {
                throw new ArgumentNullException(nameof(probableFriend));
            }

            if (!Enum.IsDefined(typeof(FriendshipState), state))
            {
                throw new Exception("Invalid FriendshipState value: " + state);
            }

            UserId = user.UserId;
            TenantId = user.TenantId;
            FriendUserId = probableFriend.UserId;
            FriendTenantId = probableFriend.TenantId;
            FriendTenancyName = probableFriendTenancyName;
            FriendUserName = probableFriendUserName;
            State = state;
            FriendProfilePictureId = probableFriendProfilePictureId;

            CreationTime = Clock.Now;
        }

        protected Friendship()
        {

        }
    }
}
