using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Users
{
    public class UsersViewModel
    {
        public string FilterText { get; set; }

        public List<ComboboxItemDto> Permissions { get; set; }

        public List<ComboboxItemDto> Roles { get; set; }

        public bool OnlyLockedUsers { get; set; }
    }
}