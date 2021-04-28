
// ReSharper disable once CheckNamespace

namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="BloggingAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public static class TagPermissions
    {
        /// <summary>
        /// Tag权限节点
        ///</summary>
        public const string Node = "Pages.Tag";

        /// <summary>
        /// Tag查询授权
        ///</summary>
        public const string Query = "Pages.Tag.Query";

        /// <summary>
        /// Tag创建权限
        ///</summary>
        public const string Create = "Pages.Tag.Create";

        /// <summary>
        /// Tag修改权限
        ///</summary>
        public const string Edit = "Pages.Tag.Edit";

        /// <summary>
        /// Tag删除权限
        ///</summary>
        public const string Delete = "Pages.Tag.Delete";

        /// <summary>
		/// Tag批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.Tag.BatchDelete";

        /// <summary>
        /// Tag导出Excel
        ///</summary>
        public const string ExportExcel = "Pages.Tag.ExportExcel";



        //// custom codes



        //// custom codes end

    }

}

