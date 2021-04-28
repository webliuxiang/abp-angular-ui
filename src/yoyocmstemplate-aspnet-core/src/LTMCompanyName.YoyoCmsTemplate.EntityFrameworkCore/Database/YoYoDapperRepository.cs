using Abp.Dapper.Repositories;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Uow;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Database
{
    /// <summary>
    /// dapper仓储，这里写为role类型,实际并不依赖Role,只是用依赖注入来获取dapper实例和相关函数
    /// 默认使用数据库会话为 YoyoCmsTemplateDbContext
    /// </summary>
    public class YoYoDapperRepository : DapperEfRepositoryBase<YoyoCmsTemplateDbContext, Role>, ITransientDependency
    {
        private readonly IActiveTransactionProvider _activeTransactionProvider;

        public YoYoDapperRepository(
            IActiveTransactionProvider activeTransactionProvider,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
            : base(activeTransactionProvider, currentUnitOfWorkProvider)
        {
            _activeTransactionProvider = activeTransactionProvider;
        }
    }
}
