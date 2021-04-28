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
    /// See <see cref="ProjectPermissions" /> for all permission names. Project
    ///</summary>
    public class ProjectAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

		public ProjectAuthorizationProvider()
		{

		}

        public ProjectAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public ProjectAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了Project 的权限。
			var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

			var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

			var entityPermission = administration.CreateChildPermission(ProjectPermissions.Node , L("Project"));
			entityPermission.CreateChildPermission(ProjectPermissions.Query, L("QueryProject"));
			entityPermission.CreateChildPermission(ProjectPermissions.Create, L("CreateProject"));
			entityPermission.CreateChildPermission(ProjectPermissions.Edit, L("EditProject"));
			entityPermission.CreateChildPermission(ProjectPermissions.Delete, L("DeleteProject"));
			entityPermission.CreateChildPermission(ProjectPermissions.BatchDelete, L("BatchDeleteProject"));
			entityPermission.CreateChildPermission(ProjectPermissions.ExportExcel, L("ExportExcelProject"));


		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, AppConsts.LocalizationSourceName);
		}
    }
}
