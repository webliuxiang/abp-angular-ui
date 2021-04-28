using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.UI;

using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships.DomainService;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.DomainService
{
    /// <summary>
    ///     课程分类领域服务层一个模块的核心业务逻辑
    /// </summary>
    public class CourseCategoryManager : AbpDomainService<CourseCategory>, ICourseCategoryManager
    {

        readonly CourseToCourseCategoryManager _courseToCourseCategoryManager;

        /// <summary>
        ///     CourseCategory的构造方法
        ///     通过构造函数注册服务到依赖注入容器中
        /// </summary>
        public CourseCategoryManager(IRepository<CourseCategory, long> courseCategoryRepository, CourseToCourseCategoryManager courseToCourseCategoryManager)
            : base(courseCategoryRepository)
        {
            _courseToCourseCategoryManager = courseToCourseCategoryManager;
        }


        public async Task<CourseCategory> FindByIdAsync(long id)
        {
            var entity = await EntityRepo.GetAsync(id);
            return entity;
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var result = await QueryAsNoTracking.AnyAsync(a => a.Id == id);
            return result;
        }

        public async Task<CourseCategory> CreateAsync(CourseCategory entity)
        {
            entity.Code = await GetNextChildCodeAsync(entity.ParentId);
            await ValidateCourseCategoryAsync(entity);

            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);
            return entity;
        }

       
        public async Task UpdateAsync(CourseCategory entity)
        {
            await ValidateCourseCategoryAsync(entity);

            await EntityRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(long id)
        {
            await EntityRepo.DeleteAsync(id);
            await _courseToCourseCategoryManager.RemoveAllByCategoryId(id);
        }

        public async Task BatchDelete(List<long> input)
        {
            if (input == null || input.Count == 0)
            {
                return;
            }

            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
            await _courseToCourseCategoryManager.RemoveAllByCategoryIds(input);
        }

        public async Task MoveAsync(long id, long? parentId)
        {

            var entity = await EntityRepo.GetAsync(id);
            if (entity.ParentId == parentId)
            {
                return;

            }

            //查询当前的子项内容
            var children = await FindChildrenAsync(id, true);

            //保存当前的Code码
            var oldCode = entity.Code;

            //移动课程分类
            entity.Code = await GetNextChildCodeAsync(parentId);
            entity.ParentId = parentId;

            await ValidateCourseCategoryAsync(entity);

            //更新子项的编码
            foreach (var child in children)
            {
                child.Code = OrganizationUnit.AppendCode(entity.Code, OrganizationUnit.GetRelativeCode(child.Code, oldCode));
            }
        }
    

        /// <summary>
        /// 获取课程分类编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<string> GetCodeAsync(long id)
        {
            return (await EntityRepo.GetAsync(id)).Code;
        }

        /// <summary>
        /// 获取新的课程编码
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected virtual async Task<string> GetNextChildCodeAsync(long? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                //此处复用组织单元的算法，不影响数据的产生
                return OrganizationUnit.AppendCode(parentCode, OrganizationUnit.CreateCode(1));
            }

            return OrganizationUnit.CalculateNextCode(lastChild.Code);
        }

        /// <summary>
        /// 获取最后一个分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected virtual async Task<CourseCategory> GetLastChildOrNullAsync(long? parentId)
        {
            var children = await EntityRepo.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }


        /// <summary>
        /// 查询子级分类
        /// </summary>
        /// <param name="parentId">父级分类id</param>
        /// <param name="recursive">是否无限向下查询</param>
        /// <returns></returns>
        protected async Task<List<CourseCategory>> FindChildrenAsync(long? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await EntityRepo.GetAllListAsync(ou => ou.ParentId == parentId);
            }

            if (!parentId.HasValue)
            {
                return await EntityRepo.GetAllListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await EntityRepo.GetAllListAsync(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            );
        }

        /// <summary>
        /// 验证课程分类是否重名
        /// </summary>
        /// <param name="entityCategory"></param>
        /// <returns></returns>
        protected virtual async Task ValidateCourseCategoryAsync(CourseCategory entityCategory)
        {
            var siblings = (await FindChildrenAsync(entityCategory.ParentId))
                .Where(cate => cate.Id != entityCategory.Id)
                .ToList();

            if (siblings.Any(ou => ou.Name == entityCategory.Name))
            {
                throw new UserFriendlyException(L("CourseCategoryNameDuplicateDisplayNameWarning", entityCategory.Name));
            }
        }
    }
}
