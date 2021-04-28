using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown
{
    [Table("BS_DDList")]
    public class DropdownList : Entity<string>
    {
        /// <summary>
        /// id为cd，内部指定
        /// </summary>

        public string DDType_Id { get; set; }


        [Required]
        [StringLength(256)]
        public virtual string Name_TX { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public virtual bool IsActive_YN { get; set; }


        /// <summary>
        /// Parent <see cref="OrganizationUnit"/>.
        /// Null, if this OU is root.
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual DropdownType Parent { get; set; }

        /// <summary>
        /// Parent <see cref="DDList"/> Id.
        /// Null, if this OU is root.
        /// </summary>
        public virtual string ParentId { get; set; }


        /// <summary>
        ///父Id列表
        /// </summary>
        [StringLength(200)]

        public virtual string ParentIdList { get; set; }


    }
}
