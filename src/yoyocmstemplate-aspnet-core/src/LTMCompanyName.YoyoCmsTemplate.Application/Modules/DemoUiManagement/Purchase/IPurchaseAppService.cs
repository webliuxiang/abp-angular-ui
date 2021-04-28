using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.DemoUiManagement.Purchase
{
    public interface IPurchaseAppService : IApplicationService
    {
        Task<string> CreatePay(PurchasePayInput input);
    }
}
