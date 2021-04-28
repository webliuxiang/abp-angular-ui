using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Languages.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;

        public CreateOrEditLanguageModalViewModel(GetLanguageForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}