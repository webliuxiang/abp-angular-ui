using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Editions
{
    /// <summary>
    /// 版本
    /// </summary>
    public interface IEditionAppService : IApplicationService
    {

        Task<ListResultDto<EditionListDto>> GetEditions();

        Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input);

        Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input);

        Task DeleteEdition(EntityDto input);

        Task MoveTenantsToAnotherEdition(MoveTenantsToAnotherEditionDto input);

        Task<int> GetTenantCount(int editionId);

        Task<List<SubscribableEditionComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null, bool addAllItem = false, bool onlyFree = false);




    }
}
