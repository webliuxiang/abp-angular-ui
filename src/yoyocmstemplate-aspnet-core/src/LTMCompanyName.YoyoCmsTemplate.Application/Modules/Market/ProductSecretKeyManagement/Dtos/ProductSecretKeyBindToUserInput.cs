using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Dtos
{
    public class ProductSecretKeyBindToUserInput
    {
        /// <summary>
        /// 卡密
        /// </summary>
        [Required]
        public string SecretKey { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Required]
        public decimal Money { get; set; }
    }
}
