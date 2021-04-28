using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.UI;

using L._52ABP.Common.Extensions.Enums;

using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships.DomainService;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Masuit.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using YoyoSoft.Mooc.CourseManagement.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo
{
    /// <summary>
    /// Course应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class CourseAppService : YoyoCmsTemplateAppServiceBase, ICourseAppService
    {
        //private readonly IRepository<Course, long> _courseRepository;
        //private readonly IEnumExtensionsAppService _enumExtensionsAppService;
        //private readonly IRepository<VideoResource, long> _videoResRepository;
        //private readonly CourseManager _courseManager;
        //private readonly IRepository<CourseCategory, long> _courseCategoryRepository;
        //private readonly IRepository<CourseToCourseCategory, long> _coursetoCategoryRepository;
        //private readonly IRepository<CourseVideoResource, long> _courseVideoRepository;
        //private readonly UserManager _userManager;

        readonly CourseManager _courseManager;
        readonly CourseToCourseCategoryManager _courseToCourseCategoryManager;
        readonly CourseCategoryManager _courseCategoryManager;

        readonly UserManager _userManager;


        private readonly IEnumExtensionsAppService _enumExtensionsAppService;

        public CourseAppService(CourseManager courseManager, CourseToCourseCategoryManager courseToCourseCategoryManager, CourseCategoryManager courseCategoryManager, UserManager userManager, IEnumExtensionsAppService enumExtensionsAppService)
        {
            _courseManager = courseManager;
            _courseToCourseCategoryManager = courseToCourseCategoryManager;
            _courseCategoryManager = courseCategoryManager;
            _userManager = userManager;
            _enumExtensionsAppService = enumExtensionsAppService;
        }



        [AbpAuthorize(CoursePermissions.Query)]
        [HttpPost]
        public async Task<PagedResultDto<CourseListDto>> GetPaged([FromBody] CourseQueryInput input)
        {
            var query = _courseManager.QueryAsNoTracking
                .Where(input.QueryConditions);


            var count = await query.CountAsync();


            var entityList = await query.OrderBy(input.SortConditions)
                .PageBy(input)
                .ToListAsync();

            var dtoList = ObjectMapper.Map<IReadOnlyList<CourseListDto>>(entityList);


            return new PagedResultDto<CourseListDto>(count, dtoList);
        }


        [AbpAuthorize(CoursePermissions.Query)]
        public async Task<CourseListDto> GetById(EntityDto<long> input)
        {
            var entity = await _courseManager.QueryAsNoTracking
                .FirstOrDefaultAsync(o => o.Id == input.Id);


            var dto = ObjectMapper.Map<CourseListDto>(entity);

            return dto;
        }


        [AbpAuthorize(CoursePermissions.Create, CoursePermissions.Edit)]
        public async Task<CreateOrUpdateCourseDto> GetForEdit(NullableIdDto<long> input)
        {
            var output = new CreateOrUpdateCourseDto();

            if (input.Id.HasValue)
            {
                var entity = await _courseManager.QueryAsNoTracking
                    .Where(e => e.Id == input.Id.Value)
                    .Include(e => e.CourseCategories)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new UserFriendlyException("抱歉！未找到对应课程");
                }
                output.Entity = ObjectMapper.Map<CourseDto>(entity);
                //output.CategoryIds = entity.CourseCategories.Select(e => e.CourseCategoryId).ToList();
                output.CategoryIds = await _courseToCourseCategoryManager.GetCateByCourseId(entity.Id)
                    .ToListAsync();

            }
            else
            {
                output.Entity = new CourseDto();
            }

            output.CourseStateEnum = _enumExtensionsAppService
                .GetEntityDoubleStringKeyValueList<CourseStateEnum>();
            output.CourseDisplayTypeEnum = _enumExtensionsAppService.GetEntityDoubleStringKeyValueList<CourseTypeEnum>();
            output.CourseVideoTypeEnum = _enumExtensionsAppService.GetEntityDoubleStringKeyValueList<CourseVideoTypeEnum>();

            return output;
        }


        [AbpAuthorize(CoursePermissions.Edit)]
        public async Task PublishCourse(EntityDto<long> input)
        {
            var entity = await _courseManager.FindById(input.Id);

            if (entity != null && entity.CourseState != CourseStateEnum.Publish)
            {
                entity.CourseState = CourseStateEnum.Publish;
                await _courseManager.Update(entity);
            }
            else
            {
                throw new UserFriendlyException("课程不存在，或者课程已经发布");

            }

        }


        [AbpAuthorize(CoursePermissions.Create, CoursePermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateCourseDto input)
        {
            if (input.Entity.Id.HasValue)
            {
                await Update(input.Entity);
            }
            else
            {
                input.Entity = await Create(input.Entity);
            }

            await _courseToCourseCategoryManager
                .UpdateCourseToCategory(input.Entity.Id.Value, input.CategoryIds.ToArray());
        }


        [AbpAuthorize(CoursePermissions.Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await this._courseManager.Delete(input.Id);
        }


        [AbpAuthorize(CoursePermissions.BatchDelete)]
        public async Task BatchDelete(List<long> input)
        {
            await this._courseManager.Delete(input);
        }


        [AbpAuthorize(CoursePermissions.CourseManage)]
        public async Task<PagedResultDto<QueryCourseStatisticsDto>> GetPagedCourseStatisticsAsync(QueryCourseStatisticsInput input)
        {
            var courseIdList = new List<long>();
            if (input.CatetoryId.HasValue && input.CatetoryId.Value > 0)
            {
                var category = await _courseCategoryManager.FindByIdAsync(input.CatetoryId.Value);
                if (category != null)
                {
                    courseIdList = _courseToCourseCategoryManager
                        .QueryAsNoTracking
                        .Include(e => e.CourseCategory)
                        .Where(x => x.CourseCategory.Code.StartsWith(category.Code)).Select(x => x.CourseId).ToList();
                }
            }
            var isHaveAdminRole = await _userManager.IsHaveAdminRole(AbpSession.UserId.Value);

            var query = _courseManager.QueryAsNoTracking
                .WhereIf(!string.IsNullOrEmpty(input.FilterText), x => x.Title.Contains(input.FilterText))
                .WhereIf(input.CourseVideoType.HasValue, x => x.CourseVideoType == input.CourseVideoType)
                .WhereIf(input.CatetoryId.HasValue && input.CatetoryId.Value > 0, x => courseIdList.Contains(x.Id))
                .WhereIf(!isHaveAdminRole, e => e.CourseToTeacher.Any(x => x.TeacherId == AbpSession.UserId.Value));

            var courseCount = await query.CountAsync();
            var course = await query.PageBy(input)
                .Select(e => new QueryCourseStatisticsDto()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StudentCount = 0,
                    TotalMoney = 0,
                    CourseState = e.CourseState,
                    CourseVideoType = e.CourseVideoType,
                    //TotalTime = e.CourseSections.Sum(a =>
                    //    a.CourseSection.VideoResources.Sum(d => d.VideoResource.Duration.Value)
                    //    )
                }).ToListAsync();

            //var ids = course.Select(e => e.Id).ToList();


            return new PagedResultDto<QueryCourseStatisticsDto>(
                   courseCount,
                   course
               );
        }


        [AbpAuthorize(CoursePermissions.CourseManage)]
        public async Task<CourseStatisticsDto> GetCourseStatistics()
        {
            var list = await _courseManager
                .QueryAsNoTracking
                .Select(e => e.CourseState)
                .ToListAsync();
            return new CourseStatisticsDto()
            {
                TotalCount = list.Count(),
                ClosedCount = list.Count(e => e == CourseStateEnum.Closed),
                PublishCount = list.Count(e => e == CourseStateEnum.Publish),
                WaitPublishCount = list.Count(e => e == CourseStateEnum.WaitPublish)
            };
        }


        [HttpPost]
        public async Task<CourseSectionAndClassHourStatisticalDto> GetSectionAndClassHourStatistics(long id)
        {
            return new CourseSectionAndClassHourStatisticalDto()
            {
                CourseSectionCount = await _courseManager
                    .CourseSections
                    .CountByCourseId(id),
                CourseClassHourCount = await _courseManager
                    .CourseSections
                    .ClassHour
                    .CountByCourseId(id)
            };
        }


        #region 内部函数


        [AbpAuthorize(CoursePermissions.Create)]
        protected virtual async Task<CourseDto> Create(CourseDto input)
        {
            var entity = ObjectMapper.Map<Course>(input);
            var tradeNumber = _courseManager.GetCourseMaxCode(OrderSourceType.Course);

            entity.CourseCode = tradeNumber;

            await _courseManager.Create(entity, true);


            return ObjectMapper.Map<CourseDto>(entity);
        }

        [AbpAuthorize(CoursePermissions.Edit)]
        protected virtual async Task Update(CourseDto input)
        {
            if (input.Id != null)
            {
                var entity = await _courseManager.FindById(input.Id.Value);
                ObjectMapper.Map(input, entity);
                await _courseManager.Update(entity);
            }
        }




        #endregion


    }
}


