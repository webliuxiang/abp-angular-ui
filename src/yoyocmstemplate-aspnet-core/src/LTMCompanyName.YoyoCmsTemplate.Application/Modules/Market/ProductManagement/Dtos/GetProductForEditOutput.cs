

using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos
{
    public class GetProductForEditOutput
    {

        public ProductEditDto Product { get; set; }

        public List<KeyValuePair<string, string>> Types { get; set; }
    }
}
