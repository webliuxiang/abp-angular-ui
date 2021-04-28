using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos
{
    public class CurrentUserProfileEditDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(UserConsts.MaxFullNameLength)]
        public string FullName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }


        public Guid? ProfilePictureId { get; set; }


        public virtual bool IsPhoneNumberConfirmed { get; set; }





        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { get; set; }
    }
}
