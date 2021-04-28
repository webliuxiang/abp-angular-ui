using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class HostSettingsEditDto
    {
        /// <summary>
        /// 基本设置
        /// </summary>
        [Required]
        public GeneralSettingsEditDto General { get; set; }

        /// <summary>
        /// 用户管理设置
        /// </summary>
        [Required]
        public HostUserManagementSettingsEditDto UserManagement { get; set; }

        /// <summary>
        /// 邮箱设置
        /// </summary>
        [Required]
        public EmailSettingsEditDto Email { get; set; }

        /// <summary>
        /// 租户设置
        /// </summary>
        [Required]
        public TenantManagementSettingsEditDto TenantManagement { get; set; }

        /// <summary>
        /// 安全设置
        /// </summary>
        [Required]
        public SecuritySettingsEditDto Security { get; set; }

        /// <summary>
        /// 发票管理
        /// </summary>
        public HostBillingSettingsEditDto Billing { get; set; }


        /// <summary>
        /// 样式
        /// </summary>
        [Required]
        public ThemeEditDto Theme { get; set; }
    }
}
