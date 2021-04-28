using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos
{
    public class CreateOrUpdateOrderInfoInput
    {
        [Required]
        public NeteaseOrderInfoEditDto NeteaseOrderInfo { get; set; }

    }
}
