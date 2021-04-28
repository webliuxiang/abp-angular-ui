using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.Identity;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Controllers
{
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class AuthorizeController : YoyoCmsTemplateControllerBase
    {
        private readonly ITenantCache _tenantCache;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;


        public AuthorizeController(ITenantCache tenantCache, LogInManager logInManager, SignInManager signInManager, AbpLoginResultTypeHelper abpLoginResultTypeHelper)
        {
            _tenantCache = tenantCache;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginModel)
        {

            AbpLoginResult<Tenant, User> loginResult = await
                GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password, GetTenancyNameOrNull());

            await _signInManager.SignInAsync(loginResult.Identity, loginModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            //var user = new ApplicationUser();
            //user.UserName = parameters.UserName;
            //var result = await _userManager.CreateAsync(user, parameters.Password);
            //if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
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
