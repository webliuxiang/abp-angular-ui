using System.Collections.Generic;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.DomainService
{
    public interface INeteaseOrderInfoManager
    {
        /// <summary>
        /// 批量插入,如果已存在的数据将会更新
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        Task Create(List<NeteaseOrderInfo> entitys, string platform);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(NeteaseOrderInfo entity);
      

        /// <summary>
        /// 获取最后一个订单
        /// </summary>
        /// <returns></returns>
        Task<NeteaseOrderInfo> GetLastOrder();

        /// <summary>
        /// 根据Id获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NeteaseOrderInfo> GetOrderById(long id);

        /// <summary>
        /// 更新QQ群审核状态
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <param name="isChecked">修改的值</param>
        /// <returns></returns>
        Task UpdateChecked(long id, bool isChecked);

        /// <summary>
        /// 批量更新QQ群审核状态
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>

        Task UpdateChecked(List<long> entityIds, bool isChecked);


        /// <summary>
        /// 更新码云审核状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        Task UpdateGiteeChecked(long id, bool isChecked);

        /// <summary>
        /// 批量更新码云审核状态
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        Task UpdateGiteeChecked(List<long> entityIds, bool isChecked);
    }
}
