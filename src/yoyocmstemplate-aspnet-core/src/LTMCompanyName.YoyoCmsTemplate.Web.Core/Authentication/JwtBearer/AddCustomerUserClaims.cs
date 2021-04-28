using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Extension;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer
{
    public class AddCustomerUserClaims : IAddCustomerUserClaims
    {
        /// <summary>
        /// 扩展的用户延伸字段-ABPSession
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddCustomerClaims(ClaimsIdentity claimsIdentity, User user)
        {

            await Task.Yield();


            var nameIdClaim = claimsIdentity.Claims.First(a => a.Type == ClaimTypes.NameIdentifier);


            //官方添加的Claim

            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            // 自定义的 Claim
            claimsIdentity.AddClaim(new Claim(AppClaimExtensionTypes.UserName, user.UserName));
            claimsIdentity.AddClaim(new Claim(AppClaimExtensionTypes.Email, user.EmailAddress));
        }
    }
}