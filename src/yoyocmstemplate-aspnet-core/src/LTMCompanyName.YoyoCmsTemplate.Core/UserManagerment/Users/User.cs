using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "bb123456";

        public new const int MaxPhoneNumberLength = 18;

        public string SignInToken { get; set; }

        /// <summary>
        /// 需要修改密码
        /// </summary>
        public bool NeedToChangeThePassword { get; set; }


        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static string CreateRandomEmail()
        {
            return DateTime.Now.Subtract(DateTime.UnixEpoch).TotalMilliseconds.ToString("F0") + "@52abp.com";
        }

        [Obsolete("Name属性已经不在使用，请使用UserName")]
        [Required(AllowEmptyStrings = true)]
        private new string Name { get; set; } = String.Empty;


        /// <summary>
        /// 个人头像Id
        /// </summary>
        public virtual Guid? ProfilePictureId { get; set; }



        [Obsolete("Surname属性已经不在使用，请使用UserName")]
        [NotMapped]
        [Required(AllowEmptyStrings = true)]
        private new string Surname { get; set; } = String.Empty;

        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string adminUserName = null, bool needToChangeThePassword = false)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = adminUserName ?? AdminUserName,
                //Name = adminUserName ?? AdminUserName,
                //Surname = adminUserName ?? AdminUserName,
                EmailAddress = emailAddress,
                NeedToChangeThePassword = needToChangeThePassword
            };

            user.SetNormalizedNames();

            return user;
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        /// <summary>
        /// 设置令牌过期时间
        /// </summary>
        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }








    }
}
