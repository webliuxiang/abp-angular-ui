using System.Threading.Tasks;
using Abp.Dependency;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Account;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization
{
    public interface IAuthorizeApi : ITransientDependency
    {
        Task Login(LoginViewModel loginModel);
        Task Register(RegisterViewModel registerParameters);
        Task Logout();
        Task<UserListDto> GetUserInfo();
    }
}
