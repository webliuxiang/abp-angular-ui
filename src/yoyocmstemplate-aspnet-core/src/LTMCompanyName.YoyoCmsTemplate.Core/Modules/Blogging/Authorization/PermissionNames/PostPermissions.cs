
// ReSharper disable once CheckNamespace

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="BloggingAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public static class PostPermissions
    {
        /// <summary>
        /// Post权限节点
        ///</summary>
        public const string Node = "Pages.Post";

        /// <summary>
        /// Post查询授权
        ///</summary>
        public const string Query = "Pages.Post.Query";

        /// <summary>
        /// Post创建权限
        ///</summary>
        public const string Create = "Pages.Post.Create";

        /// <summary>
        /// Post修改权限
        ///</summary>
        public const string Edit = "Pages.Post.Edit";

        /// <summary>
        /// Post删除权限
        ///</summary>
        public const string Delete = "Pages.Post.Delete";

        /// <summary>
		/// Post批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.Post.BatchDelete";

        /// <summary>
        /// Post导出Excel
        ///</summary>
        public const string ExportExcel = "Pages.Post.ExportExcel";



        //// custom codes



        //// custom codes end

    }

}

