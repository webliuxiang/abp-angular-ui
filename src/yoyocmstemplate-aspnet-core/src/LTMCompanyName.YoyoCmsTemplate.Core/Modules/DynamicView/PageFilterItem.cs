using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView
{
    /// <summary>
    /// 筛选条件项
    /// </summary>
    public class PageFilterItem : QueryCondition
    {
        /// <summary>
        /// 顺序号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// 组件参数
        /// </summary>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        /// 值发生改变通知的page-filter组件名称集合
        /// </summary>
        public List<string> ValueChange { get; set; }

        /// <summary>
        /// 是否启用,不启用将隐藏
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 默认宽度
        /// </summary>
        public int Width { get; set; }


        #region 扩展宽度

        /// <summary>
        ///  宽度, 当屏幕宽度: 小于 576px, 一行24
        /// </summary>
        public int? XsWidth { get; set; }
        /// <summary>
        /// 宽度 当屏幕宽度: ≥576px, 一行24
        /// </summary>
        public int? SmWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥768px, 一行24
        /// </summary>
        public int? MdWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥992px, 一行24
        /// </summary>
        public int? LgWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥1200px, 一行24
        /// </summary>
        public int? XlWidth { get; set; }
        /// <summary>
        /// 宽度, 当屏幕宽度: ≥1600px, 一行24
        /// </summary>
        public int? XxlWidth { get; set; }

        #endregion
    }
}
