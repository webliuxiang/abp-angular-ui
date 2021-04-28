using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Dtos
{
    public class BatchCreateProductSecretKeyInput
    {
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// 产品编码(助记码)
        /// </summary>
        [Required]
        public string ProductCode { get; set; }
    }
}
