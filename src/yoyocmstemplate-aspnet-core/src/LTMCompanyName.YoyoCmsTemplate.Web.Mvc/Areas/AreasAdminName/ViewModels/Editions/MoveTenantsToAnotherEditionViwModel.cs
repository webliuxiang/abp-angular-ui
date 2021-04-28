using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Editions
{
    public class MoveTenantsToAnotherEditionViwModel
    {
        public int EditionId { get; set; }

        public int TenantCount { get; set; }

        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }
    }
}