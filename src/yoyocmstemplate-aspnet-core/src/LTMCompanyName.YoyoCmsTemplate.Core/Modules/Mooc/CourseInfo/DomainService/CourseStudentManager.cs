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
    /// <summary>
    /// 课程学员关系管理器
    /// </summary>
    public class CourseStudentManager : AbpDomainService<CourseToStudent>
    {
        public CourseStudentManager(IRepository<CourseToStudent, long> entityRepo) : base(entityRepo)
        {
        }

        /// <summary>
        /// 课程关联学员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Create(long courseId, long studentId)
        {
            if (await IsExist(courseId, studentId))
            {
                return;
            }

            await this.EntityRepo.InsertAsync(
                    new CourseToStudent()
                    {
                        CourseId = courseId,
                        StudentId = studentId
                    }
                );
        }


        /// <summary>
        /// 查询课程对应的学员关联信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IQueryable<CourseToStudent> GetCourseToStudents(long courseId)
        {
            return QueryAsNoTracking.Where(o => o.CourseId == courseId);
        }

        /// <summary>
        /// 判断课程是否关联学员
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <param name="studentId">学员id</param>
        /// <returns></returns>
        public async Task<bool> IsExist(long courseId, long studentId)
        {
            var count = await QueryAsNoTracking
                    .Where(o => o.CourseId == courseId && o.StudentId == studentId)
                    .CountAsync();

            return count > 0;
        }
    }
}
