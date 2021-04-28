using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;

using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.Dtos;
using Masuit.Tools;
using Microsoft.AspNetCore.Mvc;
using LTMCompanyName.YoyoCmsTemplate.Authorization;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections
{
    /// <summary>
    /// 章节管理api,必须拥有编辑课程权限
    /// </summary>
    [AbpAuthorize(CoursePermissions.Create, CoursePermissions.Edit)]
    public class CourseSectionAppService : YoyoCmsTemplateAppServiceBase, IApplicationService
    {
        private readonly CourseManager _courseManager;

        public CourseSectionAppService(CourseManager courseManager)
        {
            _courseManager = courseManager;
        }

        /// <summary>
        /// 根据章节id获取章节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CourseSectionDto> GetById(long id)
        {
            var entity = await _courseManager.CourseSections.FindById(id);
            if (entity == null)
            {
                return null;
            }

            return ObjectMapper.Map<CourseSectionDto>(entity);
        }

        /// <summary>
        /// 根据课程id获取章节
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <returns>章节集合</returns>
        [HttpPost]
        public async Task<ListResultDto<CourseSectionDto>> GetSectionsByCourseId(long courseId)
        {
            var entityList = await _courseManager.GetSectionsByCourseId(courseId);

            return new ListResultDto<CourseSectionDto>(
                ObjectMapper.Map<IReadOnlyList<CourseSectionDto>>(entityList.OrderBy(o => o.Index))
                );
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<long> CreateOrUpdate(CourseSectionDto input)
        {
            if (input.Id.HasValue)
            {
                return await this.Update(input);
            }

            return await this.Create(input);
        }

        /// <summary>
        /// 删除章节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Delete(long id)
        {
            await this._courseManager.CourseSections.Delete(id);
        }

        /// <summary>
        /// 交换章节的序号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task ExchangeIndex(CourseSectionExchangeIndexDto input)
        {
            var entityAB = await this._courseManager.CourseSections.QueryAsNoTracking
                 .Where(o => o.Id == input.AId || o.Id == input.BId)
                 .ToListAsync();
            if (entityAB.Count != 2)
            {
                return;
            }

            var entityA = entityAB.Find(o => o.Id == input.AId);
            var entityB = entityAB.Find(o => o.Id == input.BId);


            var aIndex = entityA.Index;
            var bIndex = entityB.Index;

            entityA.Index = bIndex;
            entityB.Index = aIndex;

            await this._courseManager.CourseSections.Update(entityA);
            await this._courseManager.CourseSections.Update(entityB);

        }

        /// <summary>
        /// 根据课程id查询章节数量
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<int> GetSectionCountByCourseId(long courseId)
        {
            return await this._courseManager.CourseSections
                 .QueryAsNoTracking
                 .CountAsync(o => o.CoursesId == courseId);
        }

        #region 内部函数

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<long> Create(CourseSectionDto input)
        {
            var entity = this.ObjectMapper.Map<CourseSection>(input);

            await this._courseManager.CourseSections.Create(entity, true);

            return entity.Id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<long> Update(CourseSectionDto input)
        {
            var entity = await this._courseManager.CourseSections.FindById(input.Id.Value);
            ObjectMapper.Map(input, entity);

            await this._courseManager.CourseSections.Update(entity);

            return entity.Id;
        }

        #endregion 内部函数
    }
}
