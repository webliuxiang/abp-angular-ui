using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums
{
    /// <summary>
    /// 课程视频类型
    /// </summary>
    public enum CourseVideoTypeEnum
    {
        /// <summary>
        /// 录播
        /// </summary>
        [Description("录播")]
        RecordBroadcasting,

        /// <summary>
        /// 直播
        /// </summary>
        [Description("直播")]
        LiveBroadcasting
    }
}
