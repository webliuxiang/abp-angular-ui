using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.DomainService
{
    public interface ITencentOrderInfoManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitTencentOrderInfo();


        /// <summary>
        /// 创建腾讯课程的订单
        /// </summary>
        Task CreateTencentOrderInfo(List<TencentOrderInfo> modelInfos);

        /// <summary>
        /// 删除订单调度系统的功能
        /// </summary>
        /// <returns></returns>
        Task DeleteSyncCreateTencentOrderBackgroundJobsInfo();


        
        Task SyncTencentOrderToL52ABPCourseOrder(TencentOrderInfo tencentOrderInfo);
    }
}
