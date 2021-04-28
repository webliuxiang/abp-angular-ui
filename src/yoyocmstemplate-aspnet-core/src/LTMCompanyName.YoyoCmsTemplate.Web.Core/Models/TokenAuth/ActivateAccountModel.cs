using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth
{
    public class ActivateAccountModel
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }
        public ActivateType ActivateType { get; set; } = ActivateType.NewAccount;
    }

    public enum ActivateType
    {
        NewAccount,
        BindExistAccount
    }
}
