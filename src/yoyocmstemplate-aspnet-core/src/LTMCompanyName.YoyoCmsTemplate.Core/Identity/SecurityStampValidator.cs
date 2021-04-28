using Abp.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LTMCompanyName.YoyoCmsTemplate.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
             IOptions<SecurityStampValidatorOptions> options,
             SignInManager signInManager,
             ISystemClock systemClock,
             ILoggerFactory loggerFactory)
             : base(options, signInManager, systemClock, loggerFactory)
        {
        }
    }
}