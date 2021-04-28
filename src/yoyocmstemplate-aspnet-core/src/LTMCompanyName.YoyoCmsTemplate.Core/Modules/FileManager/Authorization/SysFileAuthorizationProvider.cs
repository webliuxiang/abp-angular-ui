using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;


// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="SysFilePermissions" /> for all permission names. SysFile
    ///</summary>
    public class SysFileAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public SysFileAuthorizationProvider()
        {
        }
        public SysFileAuthorizationProvider(bool multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig;
        }
        public SysFileAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了SysFile 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var entityPermission = administration.CreateChildPermission(SysFilePermissions.Node, L("SysFile"));
            entityPermission.CreateChildPermission(SysFilePermissions.Query, L("QuerySysFile"));
            entityPermission.CreateChildPermission(SysFilePermissions.Create, L("CreateSysFile"));
            entityPermission.CreateChildPermission(SysFilePermissions.Edit, L("EditSysFile"));
            entityPermission.CreateChildPermission(SysFilePermissions.Delete, L("DeleteSysFile"));
            entityPermission.CreateChildPermission(SysFilePermissions.BatchDelete, L("BatchDeleteSysFile"));
            entityPermission.CreateChildPermission(SysFilePermissions.ExportExcel, L("ExportToExcel"));

            //// custom codes

            //// custom codes end
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
