using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Dtos
{
    public class CreateOrUpdateTencentOrderInfoInput
    {
        [Required]
        public TencentOrderInfoEditDto TencentOrderInfo { get; set; }

    }
}
