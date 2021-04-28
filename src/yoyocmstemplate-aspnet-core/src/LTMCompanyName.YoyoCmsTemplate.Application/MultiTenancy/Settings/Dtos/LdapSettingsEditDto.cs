using Abp.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Settings.Dtos
{
    public class LdapSettingsEditDto
    {
        /// <summary>
        /// 模块启用
        /// </summary>
        public bool IsModuleEnabled { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DisableAuditing]
        public string Password { get; set; }
    }
}
