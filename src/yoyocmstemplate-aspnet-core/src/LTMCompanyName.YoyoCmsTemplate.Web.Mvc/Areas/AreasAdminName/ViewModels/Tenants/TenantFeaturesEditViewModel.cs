using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Tenants
{
    [AutoMapFrom(typeof(GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }

        public TenantFeaturesEditViewModel(Tenant tenant, GetTenantFeaturesEditOutput output)
        {
            Tenant = tenant;
            output.MapTo(this);
        }
    }
}