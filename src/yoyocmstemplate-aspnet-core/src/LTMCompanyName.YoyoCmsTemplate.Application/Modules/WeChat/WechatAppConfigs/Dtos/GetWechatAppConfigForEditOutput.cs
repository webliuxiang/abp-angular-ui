

using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Dtos
{
    public class GetWechatAppConfigForEditOutput
    {
        public WechatAppConfigEditDto WechatAppConfig { get; set; }

        public List<KeyValuePair<string, int>> WechatAppTypeList { get; set; }

    }
}
