

using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;

// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{


    public class MarketingAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public MarketingAuthorizationProvider()
        {

        }

        public MarketingAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public MarketingAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了Pages 的权限。 
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            var marketingModule = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_MarketingModule) ?? pages.CreateChildPermission(AppPermissions.Pages_MarketingModule, L("MarketingModule"));
           
            
            var entityPermission = marketingModule.CreateChildPermission(UserDownloadConfigPermissions.Node, L("UserDownloadConfig"));
            entityPermission.CreateChildPermission(UserDownloadConfigPermissions.Query, L("QueryUserDownloadConfig"));
            entityPermission.CreateChildPermission(UserDownloadConfigPermissions.Create, L("CreateUserDownloadConfig"));
            entityPermission.CreateChildPermission(UserDownloadConfigPermissions.Edit, L("EditUserDownloadConfig"));
            entityPermission.CreateChildPermission(UserDownloadConfigPermissions.Delete, L("DeleteUserDownloadConfig"));
            entityPermission.CreateChildPermission(UserDownloadConfigPermissions.BatchDelete, L("BatchDeleteUserDownloadConfig"));
            entityPermission.CreateChildPermission(UserDownloadConfigPermissions.ExportExcel, L("ExportExcelUserDownloadConfig"));
    marketingModule.CreateChildPermission(DownloadLogPermissions.Node, L("DownloadLog"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
