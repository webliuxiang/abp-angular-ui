


// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="OrderAuthorizationProvider" />中对权限的定义.
    ///</summary>
    public static class OrderPermissions
    {
        /// <summary>
        /// Order权限节点
        ///</summary>
        public const string Node = "Pages.Order";

        /// <summary>
        /// Order查询授权
        ///</summary>
        public const string Query = "Pages.Order.Query";

        /// <summary>
        /// Order查询授权
        ///</summary>
        public const string Update = "Pages.Order.Update";

        /// <summary>
        /// Order删除权限
        ///</summary>
        public const string Delete = "Pages.Order.Delete";

        /// <summary>
		/// Order批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.Order.BatchDelete";

        /// <summary>
        /// Order导出Excel
        ///</summary>
        public const string ExportExcel = "Pages.Order.ExportExcel";


        /// <summary>
        /// 编辑订单价格
        /// </summary>
        public const string EditPrice = "Pages.Order.EditPrice";

        /// <summary>
        /// 赠送订单
        /// </summary>
        public const string Present = "Pages.Order.Present";

    }

}

