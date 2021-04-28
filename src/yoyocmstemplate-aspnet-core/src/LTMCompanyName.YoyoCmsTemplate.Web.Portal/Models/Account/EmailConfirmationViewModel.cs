using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Account
{
    public class EmailConfirmationViewModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
