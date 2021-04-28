using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Cachings.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<HostCacheDto> Caches { get; set; }
    }
}