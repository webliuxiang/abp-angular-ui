using System.Collections.Generic;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}