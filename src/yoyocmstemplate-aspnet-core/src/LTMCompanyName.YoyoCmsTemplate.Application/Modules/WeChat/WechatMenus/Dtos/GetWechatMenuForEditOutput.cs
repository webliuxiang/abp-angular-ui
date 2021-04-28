using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMenus.Dtos
{
    public class GetWechatMenuForEditOutput
    {
        //configuration.CreateMap<ConditionalButtonGroup, MenuFull_ConditionalButtonGroup>();
        //    configuration.CreateMap<MenuFull_ConditionalButtonGroup, ConditionalButtonGroup>();

        //    configuration.CreateMap<ButtonGroupBase, MenuFull_ButtonGroup>();
        //    configuration.CreateMap<MenuFull_ButtonGroup, ButtonGroupBase>();

        /// <summary>
        /// 默认菜单
        /// (单独对象,数据结构参照 MenuFull_ButtonGroup)
        /// </summary>
        public object menu { get; set; }

        /// <summary>
        /// 有个性化菜单时显示。最新的在最前。
        /// (集合 数据结构参照 MenuFull_ConditionalButtonGroup)
        /// </summary>
        public object conditionalmenu { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public List<KeyValuePair<string, string>> MenuTypeList { get; set; }
    }
}
