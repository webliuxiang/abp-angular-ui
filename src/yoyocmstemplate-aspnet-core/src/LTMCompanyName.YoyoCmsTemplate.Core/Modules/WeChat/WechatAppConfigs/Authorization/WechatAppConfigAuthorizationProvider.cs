using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="WechatAppConfigPermissions" /> for all permission names. WechatAppConfig
    ///</summary>
    public class WechatAppConfigAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public WechatAppConfigAuthorizationProvider()
        {

        }

        public WechatAppConfigAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public WechatAppConfigAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了WechatAppConfig 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var wechat = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Wechat) ?? pages.CreateChildPermission(AppPermissions.Pages_Wechat, L("WechatManagement"));

            var entityPermission = wechat.CreateChildPermission(WechatAppConfigPermissions.Node, L("WechatAppConfig"));
            entityPermission.CreateChildPermission(WechatAppConfigPermissions.Query, L("QueryWechatAppConfig"));
            entityPermission.CreateChildPermission(WechatAppConfigPermissions.Create, L("CreateWechatAppConfig"));
            entityPermission.CreateChildPermission(WechatAppConfigPermissions.Edit, L("EditWechatAppConfig"));
            entityPermission.CreateChildPermission(WechatAppConfigPermissions.Delete, L("DeleteWechatAppConfig"));
            entityPermission.CreateChildPermission(WechatAppConfigPermissions.BatchDelete, L("BatchDeleteWechatAppConfig"));
            //entityPermission.CreateChildPermission(WechatAppConfigPermissions.ExportExcel, L("ExportExcelWechatAppConfig"));
            entityPermission.CreateChildPermission(WechatAppConfigPermissions.EditMenu, L("EditWechatMenu"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
