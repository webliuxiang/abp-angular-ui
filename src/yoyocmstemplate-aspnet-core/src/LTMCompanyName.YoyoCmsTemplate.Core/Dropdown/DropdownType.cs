using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown
{
    /// <summary>
    /// 下拉类型
    /// </summary>
    [Table("BS_DDType")]
    public class DropdownType : Entity<string>
    {
        /// <summary>
        /// id为cd，内部指定
        /// </summary>

        [Required]
        [StringLength(256)]
        public virtual string Name_TX { get; set; }

        /// <summary>
        /// 是否内置
        /// </summary>
        public virtual bool IsActive_YN { get; set; }

    }
}
