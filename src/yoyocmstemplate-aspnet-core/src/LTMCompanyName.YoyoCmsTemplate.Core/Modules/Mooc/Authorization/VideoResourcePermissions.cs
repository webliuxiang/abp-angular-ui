

 
// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
	/// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="VideoResourceAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class VideoResourcePermissions
	{
		/// <summary>
		/// VideoResource权限节点
		///</summary>
		public const string Node = "Pages.VideoResource";

		/// <summary>
		/// VideoResource查询授权
		///</summary>
		public const string Query = "Pages.VideoResource.Query";

		/// <summary>
		/// VideoResource创建权限
		///</summary>
		public const string Create = "Pages.VideoResource.Create";

		/// <summary>
		/// VideoResource修改权限
		///</summary>
		public const string Edit = "Pages.VideoResource.Edit";

		/// <summary>
		/// VideoResource删除权限
		///</summary>
		public const string Delete = "Pages.VideoResource.Delete";

        /// <summary>
		/// VideoResource批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.VideoResource.BatchDelete";

		/// <summary>
		/// VideoResource导出Excel
		///</summary>
		public const string ExportExcel="Pages.VideoResource.ExportExcel";

        /// <summary>
        /// 是否有同步阿里云的权限
        /// </summary>

        public const string SynchronizeAliyunVodAsync = "Pages.VideoResource.SynchronizeAliyun";





    }

}

