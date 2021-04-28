using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours
{
    /// <summary>
    /// 课时管理api,必须拥有编辑课程权限
    /// </summary>
    [AbpAuthorize(CoursePermissions.Create, CoursePermissions.Edit)]
    public class CourseClassHourAppService : YoyoCmsTemplateDomainServiceBase, IApplicationService
    {
        private readonly CourseClassHourManager _classHourManager;

        public CourseClassHourAppService(CourseClassHourManager classHourManager)
        {
            _classHourManager = classHourManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<long> CreateOrUpdate(CourseClassHourDto input)
        {
            if (input.Id.HasValue)
            {
                return await this.Update(input);
            }

            return await this.Create(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task Delete(long id)
        {
            await _classHourManager.Delete(id);
        }

        /// <summary>
        /// 根据章节获取课时集合
        /// </summary>
        /// <param name="courseSectionId">课时章节id</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ListResultDto<CourseClassHourDto>> GetBySectionId(long courseSectionId)
        {
            var entityList = await _classHourManager.
                GetBySectionId(courseSectionId);

            return new ListResultDto<CourseClassHourDto>(
                    ObjectMapper.Map<List<CourseClassHourDto>>(entityList.OrderBy(o => o.SortNumber))
                );
        }

        /// <summary>
        /// 根据章节获取课时集合
        /// </summary>
        /// <param name="courseSectionIdList">课时章节id集合</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ListResultDto<CourseClassHourDto>> GetBySectionIdList(List<long> courseSectionIdList)
        {
            if (courseSectionIdList == null || courseSectionIdList.Count == 0)
            {
                return null;
            }

            var entityList = await _classHourManager.GetCourseClassHoursBySectionIdList(courseSectionIdList);

            return new ListResultDto<CourseClassHourDto>(
                    ObjectMapper.Map<List<CourseClassHourDto>>(entityList.OrderBy(o => o.SortNumber))
                );
        }

        /// <summary>
        /// 根据课时id获取课时信息
        /// </summary>
        /// <param name="courseClassHourId">课时id</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<CourseClassHourDto> GetById(long courseClassHourId)
        {
            var entity = await _classHourManager
                .QueryAsNoTracking
                .FirstOrDefaultAsync(o => o.Id == courseClassHourId);

            if (entity == null)
            {
                return null;
            }

            return ObjectMapper.Map<CourseClassHourDto>(entity);
        }

        /// <summary>
        /// 根据章节id获取课时数量
        /// </summary>
        /// <param name="courseSectionId">章节id</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<int> GetClassHourCountBySectionId(long courseSectionId)
        {
            return await _classHourManager
                 .QueryAsNoTracking
                 .CountAsync(o => o.CourseSectionId == courseSectionId);
        }


        /// <summary>
        /// 交换课时序号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task ExchangeSortNum(CourseClassHourExchangeIndexDto input)
        {
            var entityAB = await this._classHourManager.QueryAsNoTracking
                .Where(o => o.Id == input.AId || o.Id == input.BId)
                .ToListAsync();

            if (entityAB.Count != 2)
            {
                return;
            }

            var entityA = entityAB.Find(o => o.Id == input.AId);
            var entityB = entityAB.Find(o => o.Id == input.BId);


            var aIndex = entityA.SortNumber;
            var bIndex = entityB.SortNumber;

            entityA.SortNumber = bIndex;
            entityB.SortNumber = aIndex;

            await this._classHourManager.Update(entityA);
            await this._classHourManager.Update(entityB);
        }

        /// <summary>
        /// 移动课时所属章节
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task MoveCourseClassHourSection(MoveCourseClassHourSectionDto input)
        {
            var entity = await this._classHourManager.FindById(input.Id);
            if (entity == null)
            {
                return;
            }

            entity.CourseSectionId = input.CourseSectionId;


            var minSortNumber = await _classHourManager
                 .QueryAsNoTracking
                 .Where(o => o.CourseSectionId == input.CourseSectionId)
                 .OrderBy(o => o.SortNumber)
                 .Select(o => o.SortNumber)
                 .FirstOrDefaultAsync();

            entity.SortNumber = minSortNumber - 1;
            await this._classHourManager.Update(entity);
        }

        #region 内部函数

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<long> Create(CourseClassHourDto input)
        {
            var entity = this.ObjectMapper.Map<CourseClassHour>(input);

            await this._classHourManager.Create(entity, true);

            return entity.Id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<long> Update(CourseClassHourDto input)
        {
            var entity = await _classHourManager.FindById(input.Id.Value);
            ObjectMapper.Map(input, entity);

            await this._classHourManager.Update(entity);

            return entity.Id;
        }

        #endregion 内部函数
    }
}
