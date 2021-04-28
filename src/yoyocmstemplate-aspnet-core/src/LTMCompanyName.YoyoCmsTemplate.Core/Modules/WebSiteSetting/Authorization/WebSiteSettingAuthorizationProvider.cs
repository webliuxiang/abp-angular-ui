

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
    /// See <see cref="WebSiteNoticePermissions" /> for all permission names.  
    /// See <see cref="BannerAdPermissions" /> for all permission names.  
    /// See <see cref="BlogrollPermissions" /> for all permission names.  
    /// See <see cref="BlogrollTypePermissions" /> for all permission names.  
     ///</summary>
    public class WebSiteSettingAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public WebSiteSettingAuthorizationProvider()
        {

        }

        public WebSiteSettingAuthorizationProvider(bool multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig;
        }

        public WebSiteSettingAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了Advertisement 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var webSiteModule = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_WebSiteModule) ?? pages.CreateChildPermission(AppPermissions.Pages_WebSiteModule, L("WebSiteSettingModule"));

            var bannerAd = webSiteModule.CreateChildPermission(BannerAdPermissions.Node, L("BannerAd"));
            bannerAd.CreateChildPermission(BannerAdPermissions.Query, L("QueryBannerAd"));
            bannerAd.CreateChildPermission(BannerAdPermissions.Create, L("CreateBannerAd"));
            bannerAd.CreateChildPermission(BannerAdPermissions.Edit, L("EditBannerAd"));
            bannerAd.CreateChildPermission(BannerAdPermissions.Delete, L("DeleteBannerAd"));
            bannerAd.CreateChildPermission(BannerAdPermissions.BatchDelete, L("BatchDeleteBannerAd"));




            var blogroll = webSiteModule.CreateChildPermission(BlogrollPermissions.Node, L("Blogroll"));
            blogroll.CreateChildPermission(BlogrollPermissions.Query, L("QueryBlogroll"));
            blogroll.CreateChildPermission(BlogrollPermissions.Create, L("CreateBlogroll"));
            blogroll.CreateChildPermission(BlogrollPermissions.Edit, L("EditBlogroll"));
            blogroll.CreateChildPermission(BlogrollPermissions.Delete, L("DeleteBlogroll"));
            blogroll.CreateChildPermission(BlogrollPermissions.BatchDelete, L("BatchDeleteBlogroll"));


            var blogrollType = webSiteModule.CreateChildPermission(BlogrollTypePermissions.Node, L("BlogrollType"));
            blogrollType.CreateChildPermission(BlogrollTypePermissions.Query, L("QueryBlogrollType"));
            blogrollType.CreateChildPermission(BlogrollTypePermissions.Create, L("CreateBlogrollType"));
            blogrollType.CreateChildPermission(BlogrollTypePermissions.Edit, L("EditBlogrollType"));
            blogrollType.CreateChildPermission(BlogrollTypePermissions.Delete, L("DeleteBlogrollType"));
            blogrollType.CreateChildPermission(BlogrollTypePermissions.BatchDelete, L("BatchDeleteBlogrollType"));


            var webSiteNotice = webSiteModule.CreateChildPermission(WebSiteNoticePermissions.Node, L("WebSiteNotice"));
            webSiteNotice.CreateChildPermission(WebSiteNoticePermissions.Query, L("QueryWebSiteNotice"));
            webSiteNotice.CreateChildPermission(WebSiteNoticePermissions.Create, L("CreateWebSiteNotice"));
            webSiteNotice.CreateChildPermission(WebSiteNoticePermissions.Edit, L("EditWebSiteNotice"));
            webSiteNotice.CreateChildPermission(WebSiteNoticePermissions.Delete, L("DeleteWebSiteNotice"));
            webSiteNotice.CreateChildPermission(WebSiteNoticePermissions.BatchDelete, L("BatchDeleteWebSiteNotice"));

            //// custom codes



            //// custom codes end
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
