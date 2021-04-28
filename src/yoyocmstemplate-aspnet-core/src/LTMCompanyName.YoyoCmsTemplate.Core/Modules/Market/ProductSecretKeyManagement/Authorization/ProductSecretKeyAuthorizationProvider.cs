using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;


// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="ProductSecretKeyPermissions" /> for all permission names. ProductSecretKey
    ///</summary>
    public class ProductSecretKeyAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

		public ProductSecretKeyAuthorizationProvider()
		{

		}

        public ProductSecretKeyAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public ProductSecretKeyAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了ProductSecretKey 的权限。
			var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

			var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

			var entityPermission = administration.CreateChildPermission(ProductSecretKeyPermissions.Node , L("ProductSecretKey"));
			entityPermission.CreateChildPermission(ProductSecretKeyPermissions.Query, L("QueryProductSecretKey"));
			entityPermission.CreateChildPermission(ProductSecretKeyPermissions.Create, L("CreateProductSecretKey"));
			entityPermission.CreateChildPermission(ProductSecretKeyPermissions.Edit, L("EditProductSecretKey"));
			entityPermission.CreateChildPermission(ProductSecretKeyPermissions.Delete, L("DeleteProductSecretKey"));
			entityPermission.CreateChildPermission(ProductSecretKeyPermissions.BatchDelete, L("BatchDeleteProductSecretKey"));
			entityPermission.CreateChildPermission(ProductSecretKeyPermissions.ExportExcel, L("ExportExcelProductSecretKey"));


		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, AppConsts.LocalizationSourceName);
		}
    }
}
