using System;
using System.Linq;
using System.Threading.Tasks;

using Abp.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService
{
    /// <summary>
    /// Product领域层的业务管理
    ///</summary>
    public class ProductManager : AbpDomainService<Product, Guid>, IProductManager
    {

        public ProductManager(IRepository<Product, Guid> entityRepo)
            : base(entityRepo)
        {
            
        }

        /// <summary>
        /// 初始化
        ///</summary>
        public void InitProduct()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码



        public async Task<Product> GetProductByCode(string productCode)
        {
            return await this.QueryAsNoTracking
                            .Where(o => o.Code == productCode)
                            .FirstOrDefaultAsync();
        }
    }
}
