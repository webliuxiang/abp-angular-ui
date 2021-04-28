using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    public class SendPasswordResetCodeInput
    {
        [Required(ErrorMessage = "该字段为必填项")]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        [EmailAddress(ErrorMessage = "请输入有效的电子邮件地址")]
         public string EmailAddress { get; set; }

        public string link { get; set; }

    }
}
