using System;
using System.Threading.Tasks;
using Abp.Runtime.Caching;

namespace LTMCompanyName.YoyoCmsTemplate.Security.Recaptcha
{


    [Obsolete("弃用，待完善功能")]

    public class YoYoRecaptchaValidator : IRecaptchaValidator
    {
        private readonly ICacheManager _cacheManager;

        public YoYoRecaptchaValidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }


        public Task Validate(string captchaResponse)
        {
            return null;
        }
    }
}
