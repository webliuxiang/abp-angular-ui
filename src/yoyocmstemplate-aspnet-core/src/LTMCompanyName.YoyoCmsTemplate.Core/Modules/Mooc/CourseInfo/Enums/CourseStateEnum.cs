using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums
{
    /// <summary>
    /// 课程发布状态枚举
    /// </summary>
    public enum CourseStateEnum
    {
      
        /// <summary>
        /// 未发布
        /// </summary>
        [Description("未发布")]
        WaitPublish=0,
        /// <summary>
        ///已关闭
        /// </summary>
        [Description("已关闭")]
        Closed=1 ,
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        Publish=2,
    }
}
