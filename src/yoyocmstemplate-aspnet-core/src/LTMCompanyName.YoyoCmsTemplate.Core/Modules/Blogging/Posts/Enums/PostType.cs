using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums
{
    /// <summary>
    /// 贴子类型
    /// </summary>
    public enum PostType
    {
        /// <summary>
        /// 原创
        /// </summary>
        [Description("原创")]
        Original,

        /// <summary>
        /// 转载
        /// </summary>
        [Description("转载")]
        Transshipment,

        /// <summary>
        /// 翻译
        /// </summary>
        [Description("翻译")]
        Translate
    }
}
