using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Tenants
{
    public class EditTenantViewModel
    {
        public TenantEditDto Tenant { get; set; }

        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public EditTenantViewModel(TenantEditDto tenant, IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            Tenant = tenant;
            EditionItems = editionItems;
        }
    }
}