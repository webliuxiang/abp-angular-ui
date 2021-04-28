

using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
// ReSharper disable once CheckNamespace

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="CoursePermissions" /> for all permission names. Course
    ///</summary>
    public class MoocAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

		public MoocAuthorizationProvider()
		{

		}

        public MoocAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public MoocAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

		public override void SetPermissions(IPermissionDefinitionContext context)
		{
            // 在这里配置了Course 的权限。 
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var moocModule = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_MoocModule) ?? pages.CreateChildPermission(AppPermissions.Pages_MoocModule, L("MoocModule"));

            var coursePermission = moocModule.CreateChildPermission(CoursePermissions.CourseManage , L("CourseManage"));
			coursePermission.CreateChildPermission(CoursePermissions.Query, L("QueryCourse"));
			coursePermission.CreateChildPermission(CoursePermissions.Create, L("CreateCourse"));
			coursePermission.CreateChildPermission(CoursePermissions.Edit, L("EditCourse"));
			coursePermission.CreateChildPermission(CoursePermissions.Delete, L("DeleteCourse"));
			coursePermission.CreateChildPermission(CoursePermissions.BatchDelete, L("BatchDeleteCourse"));
			coursePermission.CreateChildPermission(CoursePermissions.ExportExcel, L("ExportExcelCourse"));

            var videoResource = moocModule.CreateChildPermission(VideoResourcePermissions.Node , L("VideoResource"));
            videoResource.CreateChildPermission(VideoResourcePermissions.Query, L("QueryVideoResource"));
            videoResource.CreateChildPermission(VideoResourcePermissions.Create, L("CreateVideoResource"));
            videoResource.CreateChildPermission(VideoResourcePermissions.Edit, L("EditVideoResource"));
            videoResource.CreateChildPermission(VideoResourcePermissions.Delete, L("DeleteVideoResource"));
            videoResource.CreateChildPermission(VideoResourcePermissions.BatchDelete, L("BatchDeleteVideoResource"));
            videoResource.CreateChildPermission(VideoResourcePermissions.ExportExcel, L("ExportExcelVideoResource"));
            videoResource.CreateChildPermission(VideoResourcePermissions.SynchronizeAliyunVodAsync, L("SynchronizeAliyunVodAsync"));

            var courseCategory = moocModule.CreateChildPermission(CourseCategoryPermissions.Node , L("CourseCategory"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.Query, L("QueryCourseCategory"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.Create, L("CreateCourseCategory"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.Edit, L("EditCourseCategory"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.ManageCategoryTree, L("ManageCategoryTree"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.ManageCourse, L("ManageCourse"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.Delete, L("DeleteCourseCategory"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.BatchDelete, L("BatchDeleteCourseCategory"));
            courseCategory.CreateChildPermission(CourseCategoryPermissions.ExportExcel, L("ExportToExcel"));


		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, AppConsts.LocalizationSourceName);
		}
    }
}
