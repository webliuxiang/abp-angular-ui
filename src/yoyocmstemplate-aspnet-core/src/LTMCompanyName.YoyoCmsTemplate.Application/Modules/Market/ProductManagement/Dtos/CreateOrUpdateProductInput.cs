using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos
{
    public class CreateOrUpdateProductInput
    {
        [Required]
        public ProductEditDto Product { get; set; }

    }
}
