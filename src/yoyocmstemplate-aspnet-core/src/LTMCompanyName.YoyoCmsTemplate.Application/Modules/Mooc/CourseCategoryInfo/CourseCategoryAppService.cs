using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;

using AngleSharp.Common;

using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships.DomainService;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo
{
    /// <summary>
    ///     课程分类应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class CourseCategoryAppService : YoyoCmsTemplateAppServiceBase, ICourseCategoryAppService
    {
        readonly ICourseCategoryManager _categoryManager;
        readonly CourseManager _courseManager;
        readonly CourseToCourseCategoryManager _courseToCourseCategoryManager;

        public CourseCategoryAppService(ICourseCategoryManager categoryManager, CourseManager courseManager, CourseToCourseCategoryManager courseToCourseCategoryManager)
        {
            _categoryManager = categoryManager;
            _courseManager = courseManager;
            _courseToCourseCategoryManager = courseToCourseCategoryManager;
        }



        /// <summary>
        ///     获取所有的课程分类列表
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<CourseCategoryListDto>> GetAllCourseCategoriesList()
        {

            var entityList = await _categoryManager.EntityRepo.GetAllListAsync();

            var cateCourseCounts = await _courseToCourseCategoryManager.CateCourseCounts();


            var dtos = entityList.Select(item =>
            {
                var dto = ObjectMapper.Map<CourseCategoryListDto>(item);
                dto.CourseCount = cateCourseCounts.ContainsKey(item.Id) ? cateCourseCounts[item.Id] : 0;
                return dto;
            }).ToList();

            return new ListResultDto<CourseCategoryListDto>(dtos);
        }

        /// <summary>
        /// 移动分类
        /// </summary>
        [AbpAuthorize(CourseCategoryPermissions.ManageCategoryTree)]
        public virtual async Task<CourseCategoryListDto> Move(MoveCourseCategoryInput input)
        {
            await _categoryManager.MoveAsync(input.Id, input.NewParentId);

            var dto = await GetCategoryAndCourseCountDto(await _categoryManager.FindByIdAsync(input.Id));

            return dto;
        }

        /// <summary>
        /// 将课程添加到分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddCourse(AddCourseToCategoryInput input)
        {
            await _courseToCourseCategoryManager.BatchAddCourseToCategory(
                    input.CourseCategoryId,
                    input.CourseIds
                );

        }

        /// <summary>
        /// 从分类中移除课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task RemoveCourse(AddCourseToCategoryInput input)
        {
            await _courseToCourseCategoryManager.BatchRemoveCourseFromCategory(
                  input.CourseCategoryId,
                  input.CourseIds
              );
        }


        private async Task<CourseCategoryListDto> GetCategoryAndCourseCountDto(CourseCategory entity)
        {
            var dto = ObjectMapper.Map<CourseCategoryListDto>(entity);
            dto.CourseCount = await _categoryManager.QueryAsNoTracking.CountAsync(a => a.Id == entity.Id);

            return dto;
        }

        #region 增删改查

        /// <summary>
        ///     获取课程分类的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CourseCategoryPermissions.Query)]
        public async Task<PagedResultDto<CourseCategoryListDto>> GetPagedCategory(GetCourseCategorysInput input)
        {
            var query = _categoryManager.QueryAsNoTracking

                    //模糊搜索名称
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Name.Contains(input.FilterText))
                    //模糊搜索编码
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Code.Contains(input.FilterText))
                    //模糊搜索图片路径
                    .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.ImgUrl.Contains(input.FilterText))
                ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var courseCategoryList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var courseCategoryListDtos = ObjectMapper.Map<List<CourseCategoryListDto>>(courseCategoryList);

            return new PagedResultDto<CourseCategoryListDto>(count, courseCategoryListDtos);
        }

        /// <summary>
        ///     通过指定id获取CourseCategoryListDto信息
        /// </summary>
        [AbpAuthorize(CourseCategoryPermissions.Query)]
        public async Task<CourseCategoryListDto> GetById(EntityDto<long> input)
        {
            var entity = await _categoryManager.FindByIdAsync(input.Id);

            var dto = ObjectMapper.Map<CourseCategoryListDto>(entity);
            return dto;
        }

        /// <summary>
        ///     获取编辑 课程分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CourseCategoryPermissions.Create, CourseCategoryPermissions.Edit)]
        public async Task<GetCourseCategoryForEditOutput> GetForEdit(NullableIdDto<long> input)
        {
            var output = new GetCourseCategoryForEditOutput();
            CourseCategoryEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _categoryManager.FindByIdAsync(input.Id.Value);
                editDto = ObjectMapper.Map<CourseCategoryEditDto>(entity);
            }
            else
            {
                editDto = new CourseCategoryEditDto();
            }

            output.CourseCategory = editDto;
            return output;
        }

        /// <summary>
        ///     添加或者修改课程分类的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CourseCategoryPermissions.Create, CourseCategoryPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateCourseCategoryInput input)
        {
            if (input.CourseCategory.Id.HasValue)
            {
                await Update(input.CourseCategory);
            }
            else
            {
                await Create(input.CourseCategory);
            }
        }

        /// <summary>
        ///     新增课程分类
        /// </summary>
        [AbpAuthorize(CourseCategoryPermissions.Create)]
        protected virtual async Task<CourseCategoryEditDto> Create(CourseCategoryEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<CourseCategory>(input);
            //调用领域服务
            entity = await _categoryManager.CreateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            var dto = ObjectMapper.Map<CourseCategoryEditDto>(entity);
            return dto;
        }

        /// <summary>
        ///     编辑课程分类
        /// </summary>
        [AbpAuthorize(CourseCategoryPermissions.Edit)]
        protected virtual async Task Update(CourseCategoryEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _categoryManager.FindByIdAsync(input.Id.Value);
            if (entity == null)
            {
                throw new UserFriendlyException("指定课程分类不存在");
            }

            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _categoryManager.UpdateAsync(entity);
        }

        /// <summary>
        ///     删除课程分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CourseCategoryPermissions.Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _categoryManager.DeleteAsync(input.Id);
        }

        /// <summary>
        ///     批量删除CourseCategory的方法
        /// </summary>
        [AbpAuthorize(CourseCategoryPermissions.BatchDelete)]
        public async Task BatchDelete(List<long> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _categoryManager.BatchDelete(input);
        }

        public async Task<PagedResultDto<TreeMemberListDto>> GetPagedCourseListInCategoryAsync(GetCourseCategorysInput input)
        {
            var courseQuery = _courseManager.QueryAsNoTracking
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), r => r.Title.Contains(input.FilterText));

            var query = from courseCate in _courseToCourseCategoryManager.QueryAsNoTracking
                        join cate in _categoryManager.QueryAsNoTracking on courseCate.CourseCategoryId equals cate.Id
                        join course in courseQuery on courseCate.CourseId equals course.Id
                        where courseCate.CourseCategoryId == input.Id
                        select new
                        {
                            courseCate,
                            course
                        };

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<TreeMemberListDto>(
                totalCount,
                items.Select(item =>
                {

                    var dto = new TreeMemberListDto
                    {
                        Id = item.course.Id,
                        Name = item.course.Title,
                        AddedTime = item.courseCate.CreationTime
                    };

                    return dto;
                }).ToList());


        }

        /// <summary>
        /// 查询可以添加到分类中的课程列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<NameValueDto>> FindCourses(FindCoursesInput input)
        {
            var courseIdsInCategory = _courseToCourseCategoryManager.GetCateCourses(input.CategoryId);

            var query = _courseManager.QueryAsNoTracking
                .Where(course => !courseIdsInCategory.Contains(course.Id))
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                    course =>
                        course.Title.Contains(input.FilterText) ||
                        course.Intro.Contains(input.FilterText) ||
                        course.Description.Contains(input.FilterText)
                );

            var courseCount = await query.CountAsync();


            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<NameValueDto>(
                courseCount,
                items.Select(item =>
                    new NameValueDto(
                        $"{item.Title}",
                        item.Id.ToString()
                    )
                ).ToList()
            );

        }



        #endregion 增删改查

        //// custom codes

        //// custom codes end
    }
}
