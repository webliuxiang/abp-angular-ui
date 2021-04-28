using L._52ABP.Common.Extensions.Enums;

namespace LTMCompanyName.YoyoCmsTemplate.Enums
{
    public class EnumsAppService : YoyoCmsTemplateAppServiceBase, IEnumsAppService
    {
        private readonly IEnumExtensionsAppService _enumExtensionsAppService;

        public EnumsAppService(IEnumExtensionsAppService enumExtensionsAppService)
        {
            _enumExtensionsAppService = enumExtensionsAppService;
        }





    }
}