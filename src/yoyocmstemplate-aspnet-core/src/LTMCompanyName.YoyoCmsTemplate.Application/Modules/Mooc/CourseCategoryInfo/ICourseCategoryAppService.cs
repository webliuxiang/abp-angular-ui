
using System;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;



namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo
{
    /// <summary>
    /// 课程分类应用层服务的接口方法
    ///</summary>
    public interface ICourseCategoryAppService : IApplicationService
    {
        /// <summary>
		/// 获取课程分类的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CourseCategoryListDto>> GetPagedCategory(GetCourseCategorysInput input);


		/// <summary>
		/// 通过指定id获取课程分类ListDto信息
		/// </summary>
		Task<CourseCategoryListDto> GetById(EntityDto<long> input);


        /// <summary>
        /// 返回实体课程分类的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCourseCategoryForEditOutput> GetForEdit(NullableIdDto<long> input);


        /// <summary>
        /// 添加或者修改课程分类的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateCourseCategoryInput input);


        /// <summary>
        /// 删除课程分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<long> input);

		
        /// <summary>
        /// 批量删除课程分类
        /// </summary>
        Task BatchDelete(List<long> input);


		
							//// custom codes
                            /// <summary>
                            /// 获取所有的视频分类列表
                            /// </summary>
                            /// <returns></returns>
                            Task<ListResultDto<CourseCategoryListDto>> GetAllCourseCategoriesList();

                            Task<CourseCategoryListDto> Move(MoveCourseCategoryInput input);

                            Task AddCourse(AddCourseToCategoryInput input);

                            Task RemoveCourse(AddCourseToCategoryInput input);



                       Task<PagedResultDto<NameValueDto>> FindCourses(FindCoursesInput input);



                            /// <summary>
                            /// 获取分类下的课程信息
                            /// </summary>
                            /// <param name="input"></param>
                            /// <returns></returns>
      Task<PagedResultDto<TreeMemberListDto>> GetPagedCourseListInCategoryAsync(GetCourseCategorysInput input);


                            //// custom codes end
    }
}
