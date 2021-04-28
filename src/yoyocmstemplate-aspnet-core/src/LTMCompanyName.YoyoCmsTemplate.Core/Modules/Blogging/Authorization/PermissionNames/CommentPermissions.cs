

// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="BloggingAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public static class CommentPermissions
    {
        /// <summary>
        /// Comment权限节点
        ///</summary>
        public const string Node = "Pages.Comment";

        /// <summary>
        /// Comment查询授权
        ///</summary>
        public const string Query = "Pages.Comment.Query";

        /// <summary>
        /// Comment创建权限
        ///</summary>
        public const string Create = "Pages.Comment.Create";

        /// <summary>
        /// Comment修改权限
        ///</summary>
        public const string Edit = "Pages.Comment.Edit";

        /// <summary>
        /// Comment删除权限
        ///</summary>
        public const string Delete = "Pages.Comment.Delete";

        /// <summary>
		/// Comment批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.Comment.BatchDelete";

        /// <summary>
        /// Comment导出Excel
        ///</summary>
        public const string ExportExcel = "Pages.Comment.ExportExcel";



        //// custom codes



        //// custom codes end

    }

}

