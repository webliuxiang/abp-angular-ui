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
    /// See <see cref="OrderPermissions" /> for all permission names. Order
    ///</summary>
    public class OrderAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

		public OrderAuthorizationProvider()
		{

		}

        public OrderAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public OrderAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了Order 的权限。
			var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

			var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

			var entityPermission = administration.CreateChildPermission(OrderPermissions.Node , L("Order"));
			entityPermission.CreateChildPermission(OrderPermissions.Query, L("QueryOrder"));
			entityPermission.CreateChildPermission(OrderPermissions.EditPrice, L("EditPriceOrder"));
			entityPermission.CreateChildPermission(OrderPermissions.Present, L("Present"));
			entityPermission.CreateChildPermission(OrderPermissions.Delete, L("DeleteOrder"));
			entityPermission.CreateChildPermission(OrderPermissions.BatchDelete, L("BatchDeleteOrder"));
			entityPermission.CreateChildPermission(OrderPermissions.ExportExcel, L("ExportExcelOrder"));
            entityPermission.CreateChildPermission(OrderPermissions.Update, L("EditOrder"));

		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, AppConsts.LocalizationSourceName);
		}
    }
}
