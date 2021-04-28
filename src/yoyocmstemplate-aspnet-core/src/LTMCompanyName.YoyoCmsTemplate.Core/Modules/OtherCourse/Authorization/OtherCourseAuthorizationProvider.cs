using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
// ReSharper disable once CheckNamespace

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="NeteaseOrderInfoPermissions" /> for all permission names. OrderInfo
    /// See <see cref="TencentOrderInfoPermissions" /> for all permission names. OrderInfo
    ///</summary>
    public class OtherCourseAuthorizationProvider : AuthorizationProvider
    {
		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了OrderInfo 的权限。
			var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

			var otherCourse
                = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_OtherCourseModule) ?? pages.CreateChildPermission(AppPermissions.Pages_OtherCourseModule, L("OtherCourseModule"));

			var neteaseOrderPerimission =
                otherCourse.CreateChildPermission(NeteaseOrderInfoPermissions.Node , L("NeteaseOrderInfo"));
			neteaseOrderPerimission.CreateChildPermission(NeteaseOrderInfoPermissions.Query, L("QueryNeteaseOrderInfo"));
			neteaseOrderPerimission.CreateChildPermission(NeteaseOrderInfoPermissions.Edit, L("EditNeteaseOrderInfo"));


            var tencentOrderPermission =
                otherCourse.CreateChildPermission(TencentOrderInfoPermissions.Node, L("TencentOrderInfo"));
            tencentOrderPermission.CreateChildPermission(TencentOrderInfoPermissions.Query, L("QueryTencentOrderInfo"));
            tencentOrderPermission.CreateChildPermission(TencentOrderInfoPermissions.Create, L("CreateTencentOrderInfo"));
            tencentOrderPermission.CreateChildPermission(TencentOrderInfoPermissions.Edit, L("EditTencentOrderInfo"));
            tencentOrderPermission.CreateChildPermission(TencentOrderInfoPermissions.Delete, L("DeleteTencentOrderInfo"));
            tencentOrderPermission.CreateChildPermission(TencentOrderInfoPermissions.BatchDelete, L("BatchDeleteTencentOrderInfo"));
            tencentOrderPermission.CreateChildPermission(TencentOrderInfoPermissions.ExportExcel, L("ExportExcelTencentOrderInfo"));



        }

        private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, AppConsts.LocalizationSourceName);
		}
    }
}
