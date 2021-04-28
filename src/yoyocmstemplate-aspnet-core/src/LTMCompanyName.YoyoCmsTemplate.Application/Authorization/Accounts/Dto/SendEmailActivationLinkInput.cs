using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
