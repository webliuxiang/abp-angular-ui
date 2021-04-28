using System.Collections.Generic;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}