using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users
{
    /// <summary>
    /// 扩展Abpsession
    /// </summary>
    public interface IAddCustomerUserClaims : ISingletonDependency
    {


        /// <summary>
        /// 添加自定义的Claims
        /// </summary>
        /// <param name="claimsIdentity">Claim的管理器</param>
        /// <param name="user">当前登录用户的信息</param>
        /// <returns></returns>
        Task AddCustomerClaims(ClaimsIdentity claimsIdentity, User user);


    }
}