using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Dtos
{
    /// <summary>
    /// 查询输入
    /// </summary>
    public class QueryInput : IPagedResultRequest
    {
        /// <summary>
        /// 页面数据总数
        /// </summary>
        public int MaxResultCount { get; set; }
        /// <summary>
        /// 跳过的数量
        /// </summary>
        public int SkipCount { get; set; }

        /// <summary>
        /// 动态查询条件
        /// </summary>
        public List<QueryCondition> QueryConditions { get; set; }

        /// <summary>
        /// 动态排序
        /// </summary>
        public List<SortCondition> SortConditions { get; set; }
    }
}
