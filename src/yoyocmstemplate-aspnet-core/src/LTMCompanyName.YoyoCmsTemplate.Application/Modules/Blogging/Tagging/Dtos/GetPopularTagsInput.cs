using System;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos
{


    /// <summary>
    /// 获取流行标签的
    /// </summary>
    public class GetPopularTagsInput
    {

        public Guid BlogId { get; set; }

        public int ResultCount { get; set; } = 10;

        public int? MinimumPostCount { get; set; }
    }
}
