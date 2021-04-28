
// ReSharper disable once CheckNamespace

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
	/// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="MoocAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class CoursePermissions
	{
		/// <summary>
		/// Course权限节点
		///</summary>
		public const string CourseManage = "Pages.CourseManage";
	

		/// <summary>
		/// Course查询授权
		///</summary>
		public const string Query = "Pages.Course.Query";

		/// <summary>
		/// Course创建权限
		///</summary>
		public const string Create = "Pages.Course.Create";

		/// <summary>
		/// Course修改权限
		///</summary>
		public const string Edit = "Pages.Course.Edit";

		/// <summary>
		/// Course删除权限
		///</summary>
		public const string Delete = "Pages.Course.Delete";

        /// <summary>
		/// Course批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.Course.BatchDelete";

		/// <summary>
		/// Course导出Excel
		///</summary>
		public const string ExportExcel="Pages.Course.ExportExcel";

		 
		 
         
    }

}

