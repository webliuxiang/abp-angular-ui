using System;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Security.Recaptcha
{


    [Obsolete("弃用，待完善功能")]
    public interface IRecaptchaValidator
    {
        Task Validate(string captchaResponse);
    }
}
