using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Impersonation;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users.UserLink
{
    public interface IUserLinkManager
    {
        Task Link(User firstUser, User secondUser);

        Task<bool> AreUsersLinked(UserIdentifier firstUserIdentifier, UserIdentifier secondUserIdentifier);

        Task Unlink(UserIdentifier userIdentifier);

        Task<UserAccount> GetUserAccountAsync(UserIdentifier userIdentifier);

        Task<string> GetAccountSwitchToken(long targetUserId, int? targetTenantId);

        Task<UserAndIdentity> GetSwitchedUserAndIdentity(string switchAccountToken);
    }
}