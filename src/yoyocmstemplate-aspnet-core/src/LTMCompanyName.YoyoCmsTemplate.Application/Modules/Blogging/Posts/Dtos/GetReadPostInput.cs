using System;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts
{
    /// <summary>
    /// 用于访客阅读的文章请求参数，返回文章详情
    /// </summary>
    public class GetReadPostInput
    {
        public string Url { get; set; }

        public Guid BlogId { get; set; }
    }
}
