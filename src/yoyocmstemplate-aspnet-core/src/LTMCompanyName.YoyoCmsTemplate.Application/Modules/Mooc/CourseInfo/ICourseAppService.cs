using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos;
using YoyoSoft.Mooc.CourseManagement.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo
{
    /// <summary>
    /// Course应用层服务的接口方法
    ///</summary>
    public interface ICourseAppService : IApplicationService
    {
        /// <summary>
        /// 获取Course的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CourseListDto>> GetPaged(CourseQueryInput input);


        /// <summary>
        /// 通过指定id获取CourseListDto信息
        /// </summary>
        Task<CourseListDto> GetById(EntityDto<long> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreateOrUpdateCourseDto> GetForEdit(NullableIdDto<long> input);

        /// <summary>
        /// 发布课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PublishCourse(EntityDto<long> input);


        /// <summary>
        /// 添加或者修改Course的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateCourseDto input);


        /// <summary>
        /// 删除Course信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<long> input);


        /// <summary>
        /// 批量删除Course
        /// </summary>
        Task BatchDelete(List<long> input);


        #region 数据统计

        /// <summary>
        /// 章节和课时统计
        /// </summary>
        /// <param name="id">课程id</param>
        /// <returns></returns>
        Task<CourseSectionAndClassHourStatisticalDto> GetSectionAndClassHourStatistics(long id);

        /// <summary>
        /// 课程统计列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<QueryCourseStatisticsDto>> GetPagedCourseStatisticsAsync(QueryCourseStatisticsInput input);


        /// <summary>
        /// 课程统计数据
        /// </summary>
        /// <returns></returns>
        Task<CourseStatisticsDto> GetCourseStatistics();

        #endregion

    }
}
