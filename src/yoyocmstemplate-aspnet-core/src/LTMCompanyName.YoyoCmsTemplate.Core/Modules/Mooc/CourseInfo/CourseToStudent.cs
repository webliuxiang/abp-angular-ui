using Abp.Domain.Entities;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo
{
    /// <summary>
    /// 课程关联学生
    /// </summary>
    public class CourseToStudent : Entity<long>
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public virtual long? CourseId { get; set; }

        /// <summary>
        /// 学生id <see cref="User"/>
        /// </summary>
        public virtual long? StudentId { get; set; }

        public User Student { get; set; }

        public Course Course { get; set; }
    }
}
