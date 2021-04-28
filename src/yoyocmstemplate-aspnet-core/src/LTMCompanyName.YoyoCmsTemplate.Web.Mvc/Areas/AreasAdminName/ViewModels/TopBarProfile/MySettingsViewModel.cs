using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.TopBarProfile
{
    [AutoMapFrom(typeof(CurrentUserProfileEditDto))]
    public class MySettingsViewModel : CurrentUserProfileEditDto
    {
        /// <summary>
        /// 多时区
        /// </summary>
        public List<ComboboxItemDto> TimezoneItems { get; set; }
        /// <summary>
        /// 是否开启短信验证
        /// </summary>
        public bool SmsVerificationEnabled { get; set; }

        /// <summary>
        /// 是否可以修改用户名
        /// </summary>
        public bool CanChangeUserName => UserName != AbpUserBase.AdminUserName;

        public string Code { get; set; }

        public MySettingsViewModel(CurrentUserProfileEditDto currentUserProfileEditDto)
        {
            currentUserProfileEditDto.MapTo(this);
        }
    }
}