using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.DomainService;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.DomainService
{
    /// <summary>
    /// 课程章节管理
    /// </summary>
    public class CourseSectionManager : AbpDomainService<CourseSection>
    {
        /// <summary>
        /// 章节关联课时管理器
        /// </summary>
        readonly CourseClassHourManager _courseClassHourManager;



        public CourseSectionManager(IRepository<CourseSection, long> entityRepo, CourseClassHourManager courseClassHourManager) : base(entityRepo)
        {
            _courseClassHourManager = courseClassHourManager;
        }


        /// <summary>
        /// 章节关联课时管理器
        /// </summary>
        public virtual CourseClassHourManager ClassHour => _courseClassHourManager;


        public override async Task Delete(long id)
        {
            await base.Delete(id);

            await ClassHour.DeleteByCourseSectionId(id);
        }

        public override async Task Delete(List<long> idList)
        {
            await base.Delete(idList);

            await ClassHour.DeleteByCourseSectionIds(idList);
        }

        /// <summary>
        /// 根据课程id删除章节
        /// </summary>
        /// <returns></returns>
        public virtual async Task DeleteByCourseId(long courseId)
        {
            await this.EntityRepo.DeleteAsync(o => o.CoursesId == courseId);
        }


        /// <summary>
        /// 根据课程id集合删除章节
        /// </summary>
        /// <returns></returns>
        public virtual async Task DeleteByCourseIds(List<long> courseIdList)
        {
            await this.EntityRepo.DeleteAsync(o => courseIdList.Contains(o.CoursesId));
        }

        /// <summary>
        /// 根据课程id获取章节数量
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public virtual async Task<int> CountByCourseId(long courseId)
        {
            return await this.QueryAsNoTracking
                 .CountAsync(o => o.CoursesId == courseId);
        }




       

    }
}
