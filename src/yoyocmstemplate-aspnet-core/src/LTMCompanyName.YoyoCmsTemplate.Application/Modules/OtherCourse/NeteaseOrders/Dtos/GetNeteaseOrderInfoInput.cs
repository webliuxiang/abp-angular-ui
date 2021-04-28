using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos
{
    public class GetNeteaseOrderInfoInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 交易状态
        /// </summary>
        public string TransactionStatus { get; set; }

        /// <summary>
        /// 已审核
        /// </summary>
        public bool? IsChecked { get; set; }

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "OrderDate desc";
            }
        }

    }
}
