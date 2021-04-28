namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class UserLockOutSettingsEditDto
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 登陆校验错误锁定账户最大次数
        /// </summary>
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        /// <summary>
        /// 默认锁定时间
        /// </summary>
        public int DefaultAccountLockoutSeconds { get; set; }
    }
}