using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
