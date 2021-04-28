using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    /// <summary>
    ///     用户信息列表Dto
    ///     <see cref="User"/>
    /// </summary>
    public class UserListDto : EntityDto<long>, IPassivable, IHasCreationTime
    {
        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public List<UserListRoleDto> Roles { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsActive { get; set; }
    }
}
