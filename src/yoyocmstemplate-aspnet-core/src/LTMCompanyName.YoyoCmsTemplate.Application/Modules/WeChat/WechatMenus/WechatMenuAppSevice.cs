using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMenus.Dtos;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMenus
{
    /// <summary>
    /// 微信菜单管理
    /// </summary>
    [AbpAuthorize(WechatAppConfigPermissions.EditMenu)]
    public class WechatMenuAppSevice : YoyoCmsTemplateAppServiceBase, IWechatMenuAppSevice
    {
        /// <summary>
        /// 微信菜单类型
        /// </summary>
        public static List<KeyValuePair<string, string>> WechatMenuTypeList;

        const string SUCCESSED = "ok";

        private readonly IWechatAppConfigManager _wechatAppConfigManager;

        public WechatMenuAppSevice(
             IWechatAppConfigManager wechatAppConfigManager
            )
        {
            _wechatAppConfigManager = wechatAppConfigManager;
        }



        public async Task CreateOrWechatEditMenu(CreateOrEditWechatMenuInput input)
        {
            await _wechatAppConfigManager.RegisterWechatApp(input.AppId);

            var menuFull_ButtonGroup = new MenuFull_ButtonGroup()
            {
                button = input.Menu
            };

            if (input.MatchRule != null)
            {
                await this.CreateConditionalMenu(input.AppId, menuFull_ButtonGroup, input.MatchRule);
            }
            else
            {
                await this.UpdateDefaultMenu(input.AppId, menuFull_ButtonGroup);
            }

        }

        public async Task DeleteMenuConditional(string appId, string menuConditionalId)
        {
            await _wechatAppConfigManager.RegisterWechatApp(appId);

            try
            {
                var result = CommonApi.DeleteMenuConditional(appId, menuConditionalId);
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        public async Task<GetWechatMenuForEditOutput> GetMenuForEdit(string appId)
        {
            await _wechatAppConfigManager.RegisterWechatApp(appId);

            var result = CommonApi.GetMenu(appId);

            return new GetWechatMenuForEditOutput()
            {
                menu = result.menu,
                conditionalmenu = result.conditionalmenu,
                MenuTypeList = WechatMenuTypeList
            };
        }



        #region 编辑默认菜单，创建个性化菜单

        /// <summary>
        /// 更新自定义菜单(默认菜单)
        /// </summary>
        /// <param name="appId">微信appId</param>
        /// <param name="menuFull_Button">自定义菜单组</param>
        /// <returns></returns>
        private async Task UpdateDefaultMenu(string appId, MenuFull_ButtonGroup menuFull_Button)
        {
            await _wechatAppConfigManager.RegisterWechatApp(appId);
            try
            {
                var resultFull = new GetMenuResultFull()
                {
                    menu = menuFull_Button
                };

                // 数据校验
                var buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                // 立即创建
                var result = CommonApi.CreateMenu(appId, buttonGroup);

            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 创建个性化菜单
        /// </summary>
        /// <param name="appId">微信appId</param>
        /// <param name="menuFull_Button">菜单数据定义</param>
        /// <param name="menuMatchRule">个性化匹配规则</param>
        /// <returns></returns>
        private async Task CreateConditionalMenu(string appId, MenuFull_ButtonGroup menuFull_Button, MenuMatchRule menuMatchRule)
        {
            await _wechatAppConfigManager.RegisterWechatApp(appId);

            try
            {
                var resultFull = new GetMenuResultFull()
                {
                    menu = menuFull_Button
                };

                // 数据校验
                var buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ConditionalButtonGroup()).menu;
                // 附加规则
                var addConditionalButtonGroup = buttonGroup as ConditionalButtonGroup;
                addConditionalButtonGroup.matchrule = menuMatchRule;
                // 立即创建
                var result = CommonApi.CreateMenuConditional(appId, addConditionalButtonGroup);
            }
            catch (UserFriendlyException e)
            {
                throw e;
            }
        }

        #endregion
    }
}
