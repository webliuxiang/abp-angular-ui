using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.DomainService
{
    /// <summary>
    /// 课时管理
    /// </summary>
    public class CourseClassHourManager : AbpDomainService<CourseClassHour>
    {
        public CourseClassHourManager(IRepository<CourseClassHour, long> entityRepo)
            : base(entityRepo)
        {
        }

        /// <summary>
        /// 根据章节id删除课时
        /// </summary>
        /// <param name="courseSectionId"></param>
        /// <returns></returns>
        public async Task DeleteByCourseSectionId(long courseSectionId)
        {
            await this.EntityRepo.DeleteAsync(o => o.CourseSectionId == courseSectionId);
        }

        /// <summary>
        /// 根据章节id集合删除课时
        /// </summary>
        /// <param name="courseSectionIdList"></param>
        /// <returns></returns>
        public async Task DeleteByCourseSectionIds(List<long> courseSectionIdList)
        {
            if (courseSectionIdList == null || courseSectionIdList.Count == 0)
            {
                return;
            }

            await this.EntityRepo.DeleteAsync(o => courseSectionIdList.Contains(o.CourseSectionId));
        }


        /// <summary>
        /// 根据课程id获取课时数量
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public virtual async Task<int> CountByCourseId(long courseId)
        {
            return await this.QueryAsNoTracking
                 .CountAsync(o => o.CourseId == courseId);
        }

        /// <summary>
        /// 根据章节id获取课时数量
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public virtual async Task<int> CountByCourseSectionId(long courseSectionId)
        {
            return await this.QueryAsNoTracking
                 .CountAsync(o => o.CourseSectionId == courseSectionId);
        }

        /// <summary>
        /// 根据章节获取课时集合
        /// </summary>
        /// <param name="courseSectionIdList">课时章节id集合</param>
        /// <returns></returns>   
        public virtual async Task<List<CourseClassHour>> GetCourseClassHoursBySectionIdList(List<long> courseSectionIdList)
        {
            if (courseSectionIdList == null || courseSectionIdList.Count == 0)
            {
                return null;
            }

            var entityList = await QueryAsNoTracking
                .Where(o => courseSectionIdList.Contains(o.CourseSectionId))
                .ToListAsync();


            return entityList;
        }

        /// <summary>
        /// 根据章节获取课时集合
        /// </summary>
        /// <param name="courseSectionId">课时章节id</param>
        /// <returns></returns>
      
        public virtual async Task<List<CourseClassHour>> GetBySectionId(long courseSectionId)
        {
            var entityList = await 
                QueryAsNoTracking
                .Where(o => o.CourseSectionId == courseSectionId)
                .ToListAsync();

            return entityList;
        }


    }
}
