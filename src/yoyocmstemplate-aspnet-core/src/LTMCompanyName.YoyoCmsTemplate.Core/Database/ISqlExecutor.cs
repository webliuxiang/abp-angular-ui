using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Database
{
    /// <summary>
    /// 默认的Sql执行器
    /// </summary>
    public interface ISqlExecutor : IDomainService
    {
        /// <summary>
        /// 执行一条sql语句并将查询结果返回
        /// </summary>
        /// <typeparam name="TAny">查询的实体类型</typeparam>
        /// <param name="query">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>查询结果</returns>
        IEnumerable<TAny> Query<TAny>(string query, object parameters = null)
            where TAny : class;

        /// <summary>
        /// [异步]执行一条sql语句并将查询结果返回
        /// </summary>
        /// <typeparam name="TAny">查询的实体类型</typeparam>
        /// <param name="query">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>查询结果</returns>
        Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object parameters = null)
            where TAny : class;

        /// <summary>
        /// 执行一条sql语句并将执行结果数量返回
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>执行结果数量</returns>
        int Execute(string query, object parameters = null);

        /// <summary>
        /// [异步]执行一条sql语句并将执行结果数量返回
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>执行结果数量</returns>
        Task<int> ExecuteAsync(string query, object parameters = null);
    }
}
