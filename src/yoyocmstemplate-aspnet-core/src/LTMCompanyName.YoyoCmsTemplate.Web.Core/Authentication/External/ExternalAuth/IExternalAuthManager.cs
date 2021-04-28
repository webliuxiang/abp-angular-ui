using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth
{
    public interface IExternalAuthManager
    {
        Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode);

        Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode);
    }
}
