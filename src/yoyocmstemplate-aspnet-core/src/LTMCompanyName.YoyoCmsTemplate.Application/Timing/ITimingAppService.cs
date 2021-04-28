using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Timing.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Timing
{
    public interface ITimingAppService : IApplicationService
    {
        Task<ListResultDto<NameValueDto>> GetTimezones(GetTimezonesInput input);

        Task<List<ComboboxItemDto>> GetTimezoneComboboxItems(GetTimezoneComboboxItemsInput input);
    }
}