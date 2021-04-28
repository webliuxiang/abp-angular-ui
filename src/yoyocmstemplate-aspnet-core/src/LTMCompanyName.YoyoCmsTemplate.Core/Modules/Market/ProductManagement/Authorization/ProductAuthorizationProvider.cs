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
    /// See <see cref="ProductPermissions" /> for all permission names. Product
    ///</summary>
    public class ProductAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

		public ProductAuthorizationProvider()
		{

		}

        public ProductAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public ProductAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了Product 的权限。
			var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

			var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

			var entityPermission = administration.CreateChildPermission(ProductPermissions.Node , L("Product"));
			entityPermission.CreateChildPermission(ProductPermissions.Query, L("QueryProduct"));
			entityPermission.CreateChildPermission(ProductPermissions.Create, L("CreateProduct"));
			entityPermission.CreateChildPermission(ProductPermissions.Edit, L("EditProduct"));
			entityPermission.CreateChildPermission(ProductPermissions.Delete, L("DeleteProduct"));
			entityPermission.CreateChildPermission(ProductPermissions.BatchDelete, L("BatchDeleteProduct"));
			entityPermission.CreateChildPermission(ProductPermissions.ExportExcel, L("ExportExcelProduct"));


		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, AppConsts.LocalizationSourceName);
		}
    }
}
