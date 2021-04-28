using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth
{
    public interface IExternalAuthProviderApi
    {
        ExternalLoginProviderInfo ProviderInfo { get; }

        Task<bool> IsValidUser(string userId, string accessCode);

        Task<ExternalAuthUserInfo> GetUserInfo(string accessCode);

        void Initialize(ExternalLoginProviderInfo providerInfo);
    }
}
