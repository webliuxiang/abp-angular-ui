using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Runtime.Session;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Extension;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly LogInManager _logInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly SignInManager _signInManager;

        public IdentityAuthenticationStateProvider(LogInManager logInManager, AbpLoginResultTypeHelper abpLoginResultTypeHelper, SignInManager signInManager)
        {
            _logInManager = logInManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _signInManager = signInManager;
        }

        public IAbpSession AbpSession { get; set; }



        public async Task Login(LoginViewModel input)
        {
            NotifyAuthenticationStateChanged(this.LoginAsync(input));
        }

        public async Task Register(RegisterViewModel registerParameters)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        protected async Task<AuthenticationState> LoginAsync(LoginViewModel input)
        {
            var loginResult = await this.GetLoginResultAsync(input.UsernameOrEmailAddress, input.Password, null);

            await _signInManager.SignInAsync(loginResult.Identity, true);



            return new AuthenticationState(new ClaimsPrincipal(loginResult.Identity));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                if (AbpSession.UserId.HasValue)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, AbpSession.GetUserName()) };
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }


        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            AbpLoginResult<Tenant, User> loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }
    }
}
