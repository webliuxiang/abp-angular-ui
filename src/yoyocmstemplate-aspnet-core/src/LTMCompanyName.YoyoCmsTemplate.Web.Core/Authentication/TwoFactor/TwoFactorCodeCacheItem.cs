using System;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.TwoFactor
{
    [Serializable]
    public class TwoFactorCodeCacheItem
    {
        public const string CacheName = "YoYoAppTwoFactorCodeCache";

        public string Code { get; set; }

        public TwoFactorCodeCacheItem()
        {

        }

        public TwoFactorCodeCacheItem(string code)
        {
            Code = code;
        }
    }
}
