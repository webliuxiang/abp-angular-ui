using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<int?> ResolveTenantId(ResolveTenantIdInput input);

        Task<RegisterOutput> Register(RegisterInput input);


        /// <summary>
        /// 发送重置密码Code
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendPasswordResetCode(SendPasswordResetCodeInput input);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResetPasswordOutput> ResetPasswordAsync(ResetPasswordInput input);


        Task SendEmailActivationLink(SendEmailActivationLinkInput input);

        Task ActivateEmail(ActivateEmailInput input);

        Task<ImpersonateOutput> Impersonate(ImpersonateInput input);

        Task<ImpersonateOutput> BackToImpersonator();

        Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input);


        /// <summary>
        /// 获取邮箱验证的验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendEmailAddressConfirmCode(SendPasswordResetCodeInput input);


        /// <summary>
        /// 检查邮箱验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CheckEmailVerificationCode(GetEmailAddressCodeInput input);



    }
}
