using System.Linq;
using Abp.Authorization;
using Abp.Localization;

// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="MemberPermissions" /> for all permission names. Member
    ///</summary>
    public class MemberAuthorizationProvider : AuthorizationProvider
    {
		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了Member 的权限。
			var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

			var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

			var entityPermission = administration.CreateChildPermission(MemberPermissions.Node , L("Member"));
			entityPermission.CreateChildPermission(MemberPermissions.Query, L("QueryMember"));
			entityPermission.CreateChildPermission(MemberPermissions.Create, L("CreateMember"));
			entityPermission.CreateChildPermission(MemberPermissions.Edit, L("EditMember"));
			entityPermission.CreateChildPermission(MemberPermissions.Delete, L("DeleteMember"));
			entityPermission.CreateChildPermission(MemberPermissions.BatchDelete, L("BatchDeleteMember"));
			entityPermission.CreateChildPermission(MemberPermissions.ExportExcel, L("ExportExcelMember"));



			 
			 
			 
		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, AppConsts.LocalizationSourceName);
		}
    }
}
