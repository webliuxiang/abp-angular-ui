using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}