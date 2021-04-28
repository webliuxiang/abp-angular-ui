using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships.DomainService
{
    public class CourseToCourseCategoryManager : AbpDomainService<CourseToCourseCategory>
    {
        public CourseToCourseCategoryManager(IRepository<CourseToCourseCategory, long> entityRepo)
            : base(entityRepo)
        {
        }


        /// <summary>
        /// 查询分类对应的课程数量
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<long, int>> CateCourseCounts()
        {
            var result = new Dictionary<long, int>();

            var queryResult = await QueryAsNoTracking.GroupBy(x => x.CourseCategoryId)
                  .Select(groupCourses => new { cateId = groupCourses.Key, count = groupCourses.Count() })
                  .ToListAsync();

            foreach (var item in queryResult)
            {
                result[item.cateId] = item.count;
            }

            return result;
        }

        /// <summary>
        /// 获取分类id拥有的课程id
        /// </summary>
        /// <param name="courseCategoryId">课程分类id</param>
        /// <returns></returns>
        public IQueryable<long> GetCateCourses(long courseCategoryId)
        {
            return QueryAsNoTracking
                .Where(courseCate => courseCate.CourseCategoryId == courseCategoryId)
                .Select(courseCate => courseCate.CourseId);
        }

        /// <summary>
        /// 根据课程id获取所有分类id
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <returns></returns>
        public IQueryable<long> GetCateByCourseId(long courseId)
        {
            return QueryAsNoTracking
                .Where(o => o.CourseId == courseId)
                .Select(o => o.CourseCategoryId);
        }

        /// <summary>
        /// 添加课程到分类
        /// </summary>
        /// <param name="courseCategoryId">分类id</param>
        /// <param name="courseId">课程id</param>
        /// <returns></returns>
        public async Task AddCourseToCategory(long courseCategoryId, long courseId)
        {
            if (await IsExist(courseId, courseCategoryId))
            {
                return;
            }

            await EntityRepo.InsertAsync(new CourseToCourseCategory
             {
                 CourseId = courseId,
                 CourseCategoryId = courseCategoryId
             });
        }


        /// <summary>
        /// 添加课程到分类 - 批量
        /// </summary>
        /// <param name="courseCategoryId">分类id</param>
        /// <param name="courseIds">课程id集合</param>
        /// <returns></returns>
        public async Task BatchAddCourseToCategory(long courseCategoryId, List<long> courseIds)
        {
            if (courseIds == null || courseIds.Count == 0)
            {
                return;
            }
            var createCourseIds = courseIds.Except(this.GetCateCourses(courseCategoryId));
            if (createCourseIds.Count() == 0)
            {
                return;
            }

            foreach (var courseId in createCourseIds)
            {
                await EntityRepo.InsertAsync(new CourseToCourseCategory
                {
                    CourseId = courseId,
                    CourseCategoryId = courseCategoryId
                });
            }
        }

        /// <summary>
        /// 更新课程分类
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <param name="courseCategoryIds">分类id数组</param>
        /// <returns></returns>
        public async Task UpdateCourseToCategory(long courseId,params long[] courseCategoryIds)
        {
            await this.RemoveAllByCourseId(courseId);
            foreach (var courseCategoryId in courseCategoryIds)
            {
                await EntityRepo.InsertAsync(new CourseToCourseCategory
                {
                    CourseId = courseId,
                    CourseCategoryId = courseCategoryId
                });
            }
        }


        /// <summary>
        /// 从课程分类中移除所选课程
        /// </summary>
        /// <param name="courseCategoryId">分类id</param>
        /// <param name="courseId">课程id</param>
        /// <returns></returns>
        public async Task RemoveCourseFromCategory(long courseCategoryId,long courseId)
        {
            await EntityRepo.DeleteAsync(
                    o => o.CourseId == courseId && o.CourseCategoryId == courseCategoryId
                 );
        }

        /// <summary>
        /// 从课程分类中移除所选课程 - 批量
        /// </summary>
        /// <param name="courseCategoryId">分类id</param>
        /// <param name="courseIds">课程id集合</param>
        /// <returns></returns>
        public async Task BatchRemoveCourseFromCategory(long courseCategoryId, List<long> courseIds)
        {
            if (courseIds == null || courseIds.Count == 0)
            {
                return;
            }

            await EntityRepo.DeleteAsync(o =>
                o.CourseCategoryId == courseCategoryId && courseIds.Contains(o.CourseId)
            );
        }

        /// <summary>
        /// 根据课程分类id移除所有课程关系
        /// </summary>
        /// <param name="courseCategoryId">课程分类id</param>
        /// <returns></returns>
        public async Task RemoveAllByCategoryId(long courseCategoryId)
        {
            await EntityRepo.DeleteAsync(
                   o => o.CourseCategoryId == courseCategoryId
                );
        }

        /// <summary>
        /// 根据课程分类id集合移除所有课程关系
        /// </summary>
        /// <param name="courseCategoryIds">课程分类id集合</param>
        /// <returns></returns>
        public async Task RemoveAllByCategoryIds(List<long> courseCategoryIds)
        {
            if (courseCategoryIds == null || courseCategoryIds.Count == 0)
            {
                return;
            }
            await EntityRepo.DeleteAsync(
                   o => courseCategoryIds.Contains(o.CourseCategoryId)
                );
        }

        /// <summary>
        /// 根据课程id移除所有分类关联
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task RemoveAllByCourseId(long courseId)
        {
            await EntityRepo.DeleteAsync(
                 o => o.CourseId == courseId
              );
        }

        /// <summary>
        /// 判断课程是否存在分类中
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <param name="courseCategoryId">课程分类id</param>
        /// <returns></returns>
        protected virtual async Task<bool> IsExist(long courseId, long courseCategoryId)
        {
            var result = await QueryAsNoTracking.CountAsync(
                    ccate => ccate.CourseId == courseId && ccate.CourseCategoryId == courseCategoryId
                );
            return result > 0;
        }
    }
}
