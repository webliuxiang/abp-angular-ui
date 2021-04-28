using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.Database
{
    public class DefaultSqlExecutor : ISqlExecutor, ITransientDependency
    {
        private readonly YoYoDapperRepository _dapperRepository;

        public DefaultSqlExecutor(YoYoDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        public IEnumerable<TAny> Query<TAny>(string query, object parameters = null)
            where TAny : class
        {
            return _dapperRepository.Query<TAny>(query, parameters);
        }

        public async Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object parameters = null)
            where TAny : class
        {
            return await _dapperRepository.QueryAsync<TAny>(query, parameters);
        }

        public int Execute(string query, object parameters = null)
        {
            return _dapperRepository.Execute(query, parameters);
        }

        public async Task<int> ExecuteAsync(string query, object parameters = null)
        {
            return await _dapperRepository.ExecuteAsync(query, parameters);
        }
    }

}
