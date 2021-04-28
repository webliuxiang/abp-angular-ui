using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp;
using Abp.Domain.Repositories;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService
{
    public class CourseTeacherManager : AbpDomainService<CourseToTeacher>
    {
        public CourseTeacherManager(IRepository<CourseToTeacher, long> entityRepo) : base(entityRepo)
        {
        }


        /// <summary>
        /// 创建课程教师
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Create(long courseId, long teacherId)
        {
            if (await IsExist(courseId, teacherId))
            {
                return;
            }

            await this.EntityRepo.InsertAsync(
                    new CourseToTeacher()
                    {
                        CourseId = courseId,
                        TeacherId = teacherId
                    }
                );
        }

        /// <summary>
        /// 根据课程id和教师id删除课程教师
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task Remove(long courseId, long teacherId)
        {
            await this.EntityRepo.DeleteAsync(o =>
                o.CourseId == courseId && o.TeacherId == teacherId
            );
        }

        /// <summary>
        /// 查询课程对应的教师信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IQueryable<CourseToTeacher> GetCourseToTeachers(long courseId)
        {
            return QueryAsNoTracking.Where(o => o.CourseId == courseId);
        }

        /// <summary>
        /// 判断课程是否关联教师
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <param name="teacherId">教师id</param>
        /// <returns></returns>
        public async Task<bool> IsExist(long courseId, long teacherId)
        {
            var count = await QueryAsNoTracking
                    .Where(o => o.CourseId == courseId && o.TeacherId == teacherId)
                    .CountAsync();

            return count > 0;
        }
    }
}
