

using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LTMCompanyName.YoyoCmsTemplate.Message.DomainService
{
    /// <summary>
    /// 领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class MessageHistoryManager :YoyoCmsTemplateDomainServiceBase, IMessageHistoryManager
    {
		
		private readonly IRepository<MessageHistory,int> _messageHistoryRepository;

		/// <summary>
		/// MessageHistory的构造方法
		/// 通过构造函数注册服务到依赖注入容器中
		///</summary>
	public MessageHistoryManager(IRepository<MessageHistory, int> messageHistoryRepository)	{
			_messageHistoryRepository =  messageHistoryRepository;
		}

		 #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns></returns>
        public IQueryable<MessageHistory> QueryMessageHistorys()
        {
            return _messageHistoryRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns></returns>
        public IQueryable<MessageHistory> QueryMessageHistorysAsNoTracking()
        {
            return _messageHistoryRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MessageHistory> FindByIdAsync(int id)
        {
            var entity = await _messageHistoryRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(int id)
        {
            var result = await _messageHistoryRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion

		 
		 
        public async Task<MessageHistory> CreateAsync(MessageHistory entity)
        {
            entity.Id = await _messageHistoryRepository.InsertAndGetIdAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(MessageHistory entity)
        {
            await _messageHistoryRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _messageHistoryRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<int> input)
        {
            await _messageHistoryRepository.DeleteAsync(a => input.Contains(a.Id));
        }
	}
}
