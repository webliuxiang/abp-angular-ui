using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Tenants
{
    public class CreateTenantViewModel
    {
        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public CreateTenantViewModel(IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            EditionItems = editionItems;
        }
    }
}