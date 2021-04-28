using Abp.Domain.Entities;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo
{
    /// <summary>
    /// 课程关联教师
    /// </summary>
    public class CourseToTeacher : Entity<long>
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public virtual long CourseId { get; set; }

        public virtual Course Course { get; set; }

        /// <summary>
        /// 教师id <see cref="User"/>
        /// </summary>
        public virtual long TeacherId { get; set; }

        public virtual User Teacher { get; set; }
    }
}
