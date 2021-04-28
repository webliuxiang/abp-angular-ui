using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common;


namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class CreateEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel
    {
        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }

        public CreateEditionModalViewModel(GetEditionEditOutput output, IReadOnlyList<ComboboxItemDto> editionItems, IReadOnlyList<ComboboxItemDto> freeEditionItems)
        {
            EditionItems = editionItems;
            FreeEditionItems = freeEditionItems;
            //    ObjectMapper.Map<CreateEditionModalViewModel> (output);



            output.MapTo(this);
        }
    }
}
