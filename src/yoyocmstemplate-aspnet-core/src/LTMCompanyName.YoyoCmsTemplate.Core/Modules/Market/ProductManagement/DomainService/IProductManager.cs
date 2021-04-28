using System;
using System.Linq;
using System.Threading.Tasks;

using Abp.Domain.Services;

using NPOI.HPSF;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService
{
    public interface IProductManager : I52AbpDomainService<Product, Guid>
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProduct();


        /// <summary>
        /// 根据产品编码(助记码)获取产品
        /// </summary>
        /// <param name="productCode">产品编码(助记码)</param>
        /// <returns></returns>
        Task<Product> GetProductByCode(string productCode);

    }
}
