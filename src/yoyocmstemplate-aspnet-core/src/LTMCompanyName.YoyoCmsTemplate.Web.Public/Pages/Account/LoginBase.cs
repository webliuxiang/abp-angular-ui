using System;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Account;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared;
using Microsoft.AspNetCore.Components;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Pages.Account
{
    public class LoginBase:AbpComponentBase
    {
        [Parameter]
        public LoginViewModel LoginModel { get; set; }
        [Inject]
        private   LogInManager LogInManager { get; set; }
        [Inject]

        private   ITenantCache TenantCache { get; set; }
        [Inject]
        private SignInManager SignInManager { get; set; }
        [Inject]
        private IdentityAuthenticationStateProvider authStateProvider { get; set; }
 
        protected override async Task OnInitializedAsync()
        {

            LoginModel=new LoginViewModel();

        }
        protected async Task HandleValidSubmitAsync()
        {

        string    error = null;
            try
            {
                await authStateProvider.Login(LoginModel);
             
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }


            Console.WriteLine("OnValidSubmit");
        }


        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return TenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }
    }
}
