using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums
{
    /// <summary>
    /// 课程连载状态 独立课程/连载中课程/已完结课程
    /// </summary>
    public enum CourseTypeEnum
    {
        [Description("独立课程")]
        Ordinary=0,

        [Description("连载中课程")]
        Updateing=1 ,

        [Description("已完结课程")]
        Finish =2
    }
}
