using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users
{
    /// <summary>
    /// 给用户凭据自定义添加字段
    /// </summary>
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {

        private IAddCustomerUserClaims _addCustomerUserClaims;


        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor, IAddCustomerUserClaims addCustomerUserClaims)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
            _addCustomerUserClaims = addCustomerUserClaims;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identities.First();
            //添加信息到Claim中

            await _addCustomerUserClaims.AddCustomerClaims(identity, user);




            return principal;
        }
    }
}
