using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.UserEmail
{
    public interface IUserEmailer
    {
        /// <summary>
        /// 发送电子邮件激活链接到用户的电子邮件地址。
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// 可以设置为用户的普通密码，以包含在电子邮件中。
        /// </param>
        Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null);

        /// <summary>
        /// 发送密码重置链接到用户的电子邮件。
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">密码重置链接(可选)</param>
        Task SendPasswordResetLinkAsync(User user, string link = null);

        /// <summary>
        /// 发送邮箱验证码的
        /// </summary>
        /// <returns></returns>
        Task SendEmailAddressConfirmCode(string emailAddress, string code);



    }
}
