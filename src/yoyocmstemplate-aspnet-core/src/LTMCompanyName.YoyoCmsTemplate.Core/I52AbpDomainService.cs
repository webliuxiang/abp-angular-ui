

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;

using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate
{


    /// <summary>
    /// 52abp-pro提供的泛型领域服务
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface I52AbpDomainService<TEntity, TPrimaryKey> : IDomainService
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 实体
        /// </summary>
        IRepository<TEntity, TPrimaryKey> EntityRepo { get; }

        /// <summary>
        /// 查询器
        /// </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary>
        /// 查询器 - 不追踪
        /// </summary>
        IQueryable<TEntity> QueryAsNoTracking { get; }

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindById(TPrimaryKey id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="createAndGetId">是否获取id</param>
        /// <returns></returns>
        Task Create(TEntity entity, bool createAndGetId = false);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(TPrimaryKey id);

        /// <summary>
        /// 删除 - 按条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task Delete(List<TPrimaryKey> idList);
    }

    public interface I52AbpDomainService<TEntity> : I52AbpDomainService<TEntity, long>
      where TEntity : class, IEntity<long>
    {

    }

    /// <summary>
    /// 提供具体服务的抽象方法
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class AbpDomainService<TEntity, TPrimaryKey> : AbpServiceBase, I52AbpDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual IRepository<TEntity, TPrimaryKey> EntityRepo { get; private set; }

        public virtual IQueryable<TEntity> Query => EntityRepo.GetAll();

        public virtual IQueryable<TEntity> QueryAsNoTracking => Query.AsNoTracking();

        public virtual IAbpSession AbpSession { get; set; }

        public AbpDomainService(IRepository<TEntity, TPrimaryKey> entityRepo)
        {
            EntityRepo = entityRepo;
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }

        public virtual async Task<TEntity> FindById(TPrimaryKey id)
        {
            return await this.EntityRepo.FirstOrDefaultAsync(id);
        }

        public virtual async Task Create(TEntity entity, bool createAndGetId = false)
        {
            switch (createAndGetId)
            {
                case true:
                    await this.EntityRepo.InsertAndGetIdAsync(entity);
                    break;
                case false:
                    await this.EntityRepo.InsertAsync(entity);
                    break;
            }

        }

        public virtual async Task Update(TEntity entity)
        {
            await this.EntityRepo.UpdateAsync(entity);
        }

        public virtual async Task Delete(TPrimaryKey id)
        {
            await this.EntityRepo.DeleteAsync(id);
        }

        public virtual async Task Delete(List<TPrimaryKey> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return;
            }

            await this.EntityRepo.DeleteAsync(o => idList.Contains(o.Id));
        }

        public async Task Delete(Expression<Func<TEntity, bool>> predicate)
        {
            await this.EntityRepo.DeleteAsync(predicate);
        }

        public async Task<bool> Exist(TPrimaryKey id)
        {
            var entity = await FindById(id);
            return entity != null;
        }


    }

    public abstract class AbpDomainService<TEntity> : AbpDomainService<TEntity, long>
       where TEntity : class, IEntity<long>
    {
        public AbpDomainService(IRepository<TEntity, long> entityRepo)
            : base(entityRepo)
        {
        }
    }

}
