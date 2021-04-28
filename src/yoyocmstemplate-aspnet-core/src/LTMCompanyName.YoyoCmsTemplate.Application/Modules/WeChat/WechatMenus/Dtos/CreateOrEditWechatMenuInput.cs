using System.Collections.Generic;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Menu;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMenus.Dtos
{
    /// <summary>
    /// 创建或编辑菜单
    /// </summary>
    public class CreateOrEditWechatMenuInput
    {
        /// <summary>
        /// 应用key
        /// </summary>
        //[Required]
        public string AppId { get; set; }

        /// <summary>
        /// 菜单数据结构
        /// </summary>
        //[Required]
        public List<MenuFull_RootButton> Menu { get; set; }


        /// <summary>
        /// 匹配规则，当菜单是个性化菜单的时候输入
        /// </summary>
        public MenuMatchRule MatchRule { get; set; }

    }
}
