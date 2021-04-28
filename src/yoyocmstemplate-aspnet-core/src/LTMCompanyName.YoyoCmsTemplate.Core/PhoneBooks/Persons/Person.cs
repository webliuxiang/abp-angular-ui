using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Persons
{
    /// <summary>
    /// 人员
    /// </summary>

    public class Person : FullAuditedEntity<Guid>
    {

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(AppConsts.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [EmailAddress]
        [MaxLength(AppConsts.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 地址信息
        /// </summary>
        [MaxLength(AppConsts.MaxAddressLength)]
        public string Address { get; set; }




    }
}
