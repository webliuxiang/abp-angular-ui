using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Dtos
{
    public class CreateOrUpdateWechatAppConfigInput
    {
        [Required]
        public WechatAppConfigEditDto WechatAppConfig { get; set; }

    }
}
