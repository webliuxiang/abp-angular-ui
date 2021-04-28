using Abp.Auditing;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos
{
    /// <summary>
    ///     A helper class to store an <see cref="AuditLog" /> and a <see cref="User" /> object.
    /// </summary>
    public class AuditLogAndUser
    {
        /// <summary>
        ///  审计日志信息
        /// </summary>
        public AuditLog AuditLogInfo { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public User UserInfo { get; set; }
    }
}