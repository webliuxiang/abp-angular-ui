using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Extension;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.DomainService
{
    /// <summary>
    /// TencentOrderInfo领域层的业务管理
    ///</summary>
    public class TencentOrderInfoManager : YoyoCmsTemplateDomainServiceBase, ITencentOrderInfoManager
    {

        private readonly IRepository<TencentOrderInfo, long> _tencentOrderInfoRepository;
        private readonly IRepository<Order, Guid> _orderRepository;


        
        public TencentOrderInfoManager(
            IRepository<TencentOrderInfo, long> tencentOrderInfoRepository, 
            IRepository<Order, Guid> orderRepository
            )
        {
            _tencentOrderInfoRepository = tencentOrderInfoRepository;
            _orderRepository = orderRepository;
        }


        /// <summary>
		/// 初始化
		///</summary>
		public void InitTencentOrderInfo()
		{
			throw new NotImplementedException();
		}

        public async Task CreateTencentOrderInfo(List<TencentOrderInfo> modelInfos)
        {

            var newOrganizationIds = modelInfos.Select(a => a.OrganizationId).ToList();

            //查询出在数据库已经有的数据
            var dataInfos = await _tencentOrderInfoRepository.GetAll().AsNoTracking()
                .Where(a => newOrganizationIds.Contains(a.OrganizationId)).ToListAsync();


            //需要更新数据的订单信息
            var updateInfos = modelInfos.Where(a => dataInfos.Exists(d => d.OrderNumber == a.OrderNumber))
                .ToList();

            updateInfos.ForEach(a=>modelInfos.Remove(a));//排除已经在数据库中存在的数据。


            foreach (var entity in modelInfos)
            {
                await _tencentOrderInfoRepository.InsertAsync(entity);
            }


 




        }

        public async Task DeleteSyncCreateTencentOrderBackgroundJobsInfo()
        {
            await Task.Yield();
        }

        public async Task SyncTencentOrderToL52ABPCourseOrder(TencentOrderInfo tencentOrderInfo)
        {
            var orderNumber = tencentOrderInfo.OrderNumber.ToString();

       var entity=   await  _orderRepository.FirstOrDefaultAsync(a => a.OrderCode == orderNumber);

       if (entity!=null)
       {
           throw new UserFriendlyException("此订单编码已经被使用，如有疑问请联系管理员!");
       }

      

            var order = new Order
            {
                ActualPayment = 0,
                OrderCode = tencentOrderInfo.OrderNumber.ToString(),
                ProductName = "【同步】" + tencentOrderInfo.CourseTitle,
                ProductCode = "030015602643300001",
                Price = 0,
                Discounts = 0
            };

            order.ActualPayment = 0;
            order.Status = OrderStatusEnum.ChangeHands;
            order.ProductCreateProjectCount = 0;
            order.UserId = AbpSession.UserId;
            order.UserName = AbpSession.GetUserName();
            order.CourseId = 1;
            order.OrderSourceType = OrderSourceType.Course;
            

            try
            {
                await _orderRepository.InsertAsync(order);

                tencentOrderInfo.IsQqGroupChecked = true;
                tencentOrderInfo.Is52AbpChecked = true;
 await _tencentOrderInfoRepository.UpdateAsync(tencentOrderInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
      



        }

    







    }
}
