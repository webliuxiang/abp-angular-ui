using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos
{
    /// <summary>
    /// 搜索视频资源输入
    /// </summary>
    public class SearchVideoResourceInput
    {
        /// <summary>
        /// 视频分类id
        /// </summary>
        public long VideoCategoryId { get; set; }

        /// <summary>
        /// 筛选名称
        /// </summary>
        public string Filter { get; set; }
    }
}
