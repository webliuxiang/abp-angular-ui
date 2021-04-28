using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos.EntityChange
{
    /// <summary>
    /// A helper class to store an <see cref="EntityChange"/> and a <see cref="User"/> object.
    /// </summary>
    public class EntityChangeAndUser
    {
        public Abp.EntityHistory.EntityChange EntityChange { get; set; }

        public User User { get; set; }
    }
}