using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    public class DatabaseCheckHelper : ITransientDependency
    {
        private readonly IDbContextProvider<YoyoCmsTemplateDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DatabaseCheckHelper(
            IDbContextProvider<YoyoCmsTemplateDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager
        )
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public bool Exist(string connectionString)
        {
            if (connectionString.IsNullOrEmpty())
            {
                //单元测试下连接字符串为空
                return true;
            }

            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    // 在单租户模式下切换到host
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        _dbContextProvider.GetDbContext().Database.OpenConnection();
                        uow.Complete();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}