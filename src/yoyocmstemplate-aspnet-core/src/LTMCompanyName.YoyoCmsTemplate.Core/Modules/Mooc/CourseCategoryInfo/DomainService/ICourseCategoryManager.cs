
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp.Domain.Services;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.DomainService
{
    /// <summary>
    /// 课程分类管理
    /// </summary>
    public interface ICourseCategoryManager : I52AbpDomainService<CourseCategory>
    {

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CourseCategory> FindByIdAsync(long id);

        /// <summary>
        /// 根据分类id检查分类是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(long id);

        /// <summary>
        /// 添加新的课程分类
        /// </summary>
        /// <param name="entity">课程分类实体</param>
        /// <returns></returns>
        Task<CourseCategory> CreateAsync(CourseCategory entity);

     

        /// <summary>
        /// 修改课程分类
        /// </summary>
        /// <param name="entity">课程分类实体</param>
        /// <returns></returns>
        Task UpdateAsync(CourseCategory entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(long id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        Task BatchDelete(List<long> input);



        /// <summary>
        /// 移动分类
        /// </summary>
        /// <returns></returns>
        Task MoveAsync(long id, long? parentId);
    }
}
