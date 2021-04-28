using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="DropdownListPermissions" /> for all permission names. DropdownList
    ///</summary>
    public class DropdownListAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public DropdownListAuthorizationProvider()
        {

        }


        public DropdownListAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public DropdownListAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了DropdownList 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var dropdownList = administration.CreateChildPermission(DropdownListPermissions.Node, L("DropdownList"));
            dropdownList.CreateChildPermission(DropdownListPermissions.Query, L("QueryDropdownList"));
            dropdownList.CreateChildPermission(DropdownListPermissions.Create, L("CreateDropdownList"));
            dropdownList.CreateChildPermission(DropdownListPermissions.Edit, L("EditDropdownList"));
            dropdownList.CreateChildPermission(DropdownListPermissions.Delete, L("DeleteDropdownList"));
            dropdownList.CreateChildPermission(DropdownListPermissions.BatchDelete, L("BatchDeleteDropdownList"));
            dropdownList.CreateChildPermission(DropdownListPermissions.ExportExcel, L("ExportToExcel"));


            //// custom codes



            //// custom codes end
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
