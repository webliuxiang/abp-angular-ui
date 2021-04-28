
// ReSharper disable once CheckNamespace

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
	/// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="MoocAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class CourseCategoryPermissions
	{
		/// <summary>
		/// CourseCategory权限节点
		///</summary>
		public const string Node = "Pages.CourseCategory";

		/// <summary>
		/// CourseCategory查询授权
		///</summary>
		public const string Query = "Pages.CourseCategory.Query";

		/// <summary>
		/// CourseCategory创建权限
		///</summary>
		public const string Create = "Pages.CourseCategory.Create";

		/// <summary>
		/// CourseCategory修改权限
		///</summary>
		public const string Edit = "Pages.CourseCategory.Edit";
		public const string ManageCategoryTree = "Pages.CourseCategory.ManageCategoryTree";
		public const string ManageCourse = "Pages.CourseCategory.ManageCourse";

		/// <summary>
		/// CourseCategory删除权限
		///</summary>
		public const string Delete = "Pages.CourseCategory.Delete";

        /// <summary>
		/// CourseCategory批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.CourseCategory.BatchDelete";

		/// <summary>
		/// CourseCategory导出Excel
		///</summary>
		public const string ExportExcel="Pages.CourseCategory.ExportExcel";

		 
		 
							//// custom codes
									
							

							//// custom codes end
         
    }

}

