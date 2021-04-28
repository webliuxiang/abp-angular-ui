using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users.UserPolicy
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
