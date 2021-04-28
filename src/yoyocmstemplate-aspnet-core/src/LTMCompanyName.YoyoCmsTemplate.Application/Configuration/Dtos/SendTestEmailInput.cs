using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Configuration.Dtos
{
    public class SendTestEmailInput
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}
